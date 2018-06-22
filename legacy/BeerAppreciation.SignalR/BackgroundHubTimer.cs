using System;
using System.CodeDom;
using System.Globalization;
using System.Threading;
using System.Web.Hosting;
using BeerAppreciation.SignalR.Hubs;
using Microsoft.AspNet.SignalR;

namespace BeerAppreciation.SignalR
{
    public class BackgroundHubTimer: IRegisteredObject
    {
        private readonly Timer taskTimer;
        private readonly IHubContext hub;

        public BackgroundHubTimer() : this(5)
        { }

        public BackgroundHubTimer(int intervalSeconds)
        {
            IntervalSeconds = intervalSeconds;

            HostingEnvironment.RegisterObject(this);

            hub = GlobalHost.ConnectionManager.GetHubContext<ClientPushHub>();

            taskTimer = new Timer(OnTimerElapsed, null,
                TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(IntervalSeconds));
        }

        public int IntervalSeconds { get; set; }

        private void OnTimerElapsed(object sender)
        {
            hub.Clients.All.serverTime(DateTime.UtcNow);
        }

        public void Stop(bool immediate)
        {
            taskTimer.Dispose();

            HostingEnvironment.UnregisterObject(this);
        }
    }
}
