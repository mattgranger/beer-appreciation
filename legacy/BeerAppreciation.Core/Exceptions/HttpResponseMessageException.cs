namespace BeerAppreciation.Core.Exceptions
{
    using System;
    using System.Net.Http;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that will be raised when we get an error making a http request
    /// </summary>
    [Serializable]
    public class HttpResponseMessageException : Exception
    {
        #region Fields and Constants

        /// <summary>
        /// The response
        /// </summary>
        private readonly HttpResponseMessage response;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponseMessageException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="response">The response.</param>
        public HttpResponseMessageException(string message, HttpResponseMessage response)
            : base(message)
        {
            this.response = response;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponseMessageException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public HttpResponseMessageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponseMessageException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected HttpResponseMessageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
        /// </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion
    }
}
