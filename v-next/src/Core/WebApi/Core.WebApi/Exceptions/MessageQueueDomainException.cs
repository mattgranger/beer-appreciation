namespace BeerAppreciation.Core.WebApi.Exceptions
{
    using System;

    public class MessageQueueDomainException : Exception
    {
        public MessageQueueDomainException()
        { }

        public MessageQueueDomainException(string message)
            : base(message)
        { }

        public MessageQueueDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
