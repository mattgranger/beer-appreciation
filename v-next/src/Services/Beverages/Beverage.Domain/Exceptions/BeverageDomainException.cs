namespace BeerAppreciation.Beverage.Domain.Exceptions
{
    using System;

    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class BeverageDomainException : Exception
    {
        public BeverageDomainException()
        { }

        public BeverageDomainException(string message)
            : base(message)
        { }

        public BeverageDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
