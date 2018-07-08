namespace BeerAppreciation.Core.EventBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Events;

    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<InMemoryEventBusSubscriptionsManager.SubscriptionInfo>> handlers;
        private readonly List<Type> eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public InMemoryEventBusSubscriptionsManager()
        {
            this.handlers = new Dictionary<string, List<InMemoryEventBusSubscriptionsManager.SubscriptionInfo>>();
            this.eventTypes = new List<Type>();
        }

        public bool IsEmpty => !this.handlers.Keys.Any();

        public void Clear() => this.handlers.Clear();

        public void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            this.DoAddSubscription(typeof(TH), eventName, isDynamic: true);
        }

        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = this.GetEventKey<T>();
            this.DoAddSubscription(typeof(TH), eventName, isDynamic: false);
            this.eventTypes.Add(typeof(T));
        }

        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!this.HasSubscriptionsForEvent(eventName))
            {
                this.handlers.Add(eventName, new List<InMemoryEventBusSubscriptionsManager.SubscriptionInfo>());
            }

            if (this.handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            if (isDynamic)
            {
                this.handlers[eventName].Add(InMemoryEventBusSubscriptionsManager.SubscriptionInfo.Dynamic(handlerType));
            }
            else
            {
                this.handlers[eventName].Add(InMemoryEventBusSubscriptionsManager.SubscriptionInfo.Typed(handlerType));
            }
        }


        public void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            var handlerToRemove = this.FindDynamicSubscriptionToRemove<TH>(eventName);
            this.DoRemoveHandler(eventName, handlerToRemove);
        }


        public void RemoveSubscription<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent
        {
            var handlerToRemove = this.FindSubscriptionToRemove<T, TH>();
            var eventName = this.GetEventKey<T>();
            this.DoRemoveHandler(eventName, handlerToRemove);
        }


        private void DoRemoveHandler(string eventName, InMemoryEventBusSubscriptionsManager.SubscriptionInfo subsToRemove)
        {
            if (subsToRemove != null)
            {
                this.handlers[eventName].Remove(subsToRemove);
                if (!this.handlers[eventName].Any())
                {
                    this.handlers.Remove(eventName);
                    var eventType = this.eventTypes.SingleOrDefault(e => e.Name == eventName);
                    if (eventType != null)
                    {
                        this.eventTypes.Remove(eventType);
                    }
                    this.RaiseOnEventRemoved(eventName);
                }

            }
        }

        public IEnumerable<InMemoryEventBusSubscriptionsManager.SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var key = this.GetEventKey<T>();
            return this.GetHandlersForEvent(key);
        }

        public IEnumerable<InMemoryEventBusSubscriptionsManager.SubscriptionInfo> GetHandlersForEvent(string eventName) => this.handlers[eventName];

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = this.OnEventRemoved;
            if (handler != null)
            {
                this.OnEventRemoved(this, eventName);
            }
        }

        private InMemoryEventBusSubscriptionsManager.SubscriptionInfo FindDynamicSubscriptionToRemove<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            return this.DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        private InMemoryEventBusSubscriptionsManager.SubscriptionInfo FindSubscriptionToRemove<T, TH>()
             where T : IntegrationEvent
             where TH : IIntegrationEventHandler<T>
        {
            var eventName = this.GetEventKey<T>();
            return this.DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        private InMemoryEventBusSubscriptionsManager.SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!this.HasSubscriptionsForEvent(eventName))
            {
                return null;
            }

            return this.handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);

        }

        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            var key = this.GetEventKey<T>();
            return this.HasSubscriptionsForEvent(key);
        }

        public bool HasSubscriptionsForEvent(string eventName) => this.handlers.ContainsKey(eventName);

        public Type GetEventTypeByName(string eventName) => this.eventTypes.SingleOrDefault(t => t.Name == eventName);

        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }
    }
}
