namespace BeerAppreciation.Core.WebApi.Exceptions
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that will be raised for all handled service exceptions
    /// </summary>
    [Serializable]
    public class WebApiServiceException : Exception
    {
        #region Fields and Constants

        /// <summary>
        /// Storage variable for the error code property
        /// </summary>
        private readonly int? errorCode;

        /// <summary>
        /// The HttpStatusCode to return for this type of error
        /// </summary>
        private readonly HttpStatusCode httpStatusCode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiServiceException" /> class.
        /// </summary>
        public WebApiServiceException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiServiceException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WebApiServiceException(string message)
            : this(message, HttpStatusCode.InternalServerError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiServiceException" /> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        public WebApiServiceException(string message, HttpStatusCode httpStatusCode)
            : this(null, message, httpStatusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiServiceException" /> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The error message.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        public WebApiServiceException(int? errorCode, string message, HttpStatusCode httpStatusCode)
            : this(errorCode, message, httpStatusCode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiServiceException" /> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebApiServiceException(int? errorCode, string message, HttpStatusCode httpStatusCode, Exception innerException)
            : base(message, innerException)
        {
            this.errorCode = errorCode;
            this.httpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiServiceException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebApiServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiServiceException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected WebApiServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public int? ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }

        /// <summary>
        /// Gets the HTTP status code.
        /// </summary>
        public HttpStatusCode HttpStatusCode
        {
            get
            {
                return this.httpStatusCode;
            }
        }

        #endregion

        #region Overriden Methods

        #endregion
    }
}
