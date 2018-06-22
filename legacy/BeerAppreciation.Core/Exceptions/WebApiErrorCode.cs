namespace BeerAppreciation.Core.Exceptions
{
    /// <summary>
    /// Defines a unique set of common error codes that will be returned from WebApi services. The error codes are unique across all services.
    /// </summary>
    public static class WebApiErrorCode
    {
        #region Common Error Codes

        /// <summary>
        /// The unknown error, error code.
        /// </summary>
        public const int UnknownError = 1;

        /// <summary>
        /// The database concurrency error code
        /// </summary>
        public const int DatabaseConcurrencyError = 2;

        /// <summary>
        /// The validation error code
        /// </summary>
        public const int ValidationError = 3;

        #endregion

        #region TravelManagement Service Error Codes

        /// <summary>
        /// The duplicate flight booking on request error
        /// </summary>
        public const int DuplicateFlightBookingOnRequestError = 10000;

        #endregion
    }
}
