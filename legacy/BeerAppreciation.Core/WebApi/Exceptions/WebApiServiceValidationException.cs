using BeerAppreciation.Core.Exceptions;

namespace BeerAppreciation.Core.WebApi.Exceptions
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Web.Http.ModelBinding;

    /// <summary>
    /// The exception that will be raised when a validation error is encountered within the webapi layer
    /// </summary>
    [Serializable]
    public class WebApiServiceValidationException : WebApiServiceException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiServiceValidationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="modelState">State of the model containing the validation error.</param>
        public WebApiServiceValidationException(string message, ModelStateDictionary modelState)
            : base(WebApiErrorCode.ValidationError, message, HttpStatusCode.BadRequest)
        {
            this.ModelState = modelState;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiServiceValidationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="modelState">State of the model containing the validation error.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebApiServiceValidationException(string message, ModelStateDictionary modelState, Exception innerException)
            : base(message, innerException)
        {
            this.ModelState = modelState;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiServiceValidationException" /> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0).</exception>
        protected WebApiServiceValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the Model state containing the validation errors
        /// </summary>
        public ModelStateDictionary ModelState { get; private set; }

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
    }
}
