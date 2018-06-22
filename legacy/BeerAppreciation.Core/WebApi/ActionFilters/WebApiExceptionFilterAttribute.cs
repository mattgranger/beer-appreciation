namespace BeerAppreciation.Core.WebApi.ActionFilters
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;
    using System.Web.Http.ModelBinding;
    using Castle.Core.Logging;
    using Core.Exceptions;
    using Exceptions;

    /// <summary>
    /// Custom ActionFilter that will catch all unhandled webapi exceptions
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Mandatory argument should not be retrievable")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// The log4net logger
        /// </summary>
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiExceptionFilterAttribute" /> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public WebApiExceptionFilterAttribute(ILogger log)
        {
            this.log = log;
        }

        /// <summary>
        /// Gets the instance of the logger.
        /// </summary>
        public ILogger Log
        {
            get
            {
                return this.log;
            }
        }

        /// <summary>
        /// Called when an exception occurs within the WebApi layer.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "Need to get error code for WebApiServiceException")]
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
            Exception exception = actionExecutedContext.Exception;

            // Convert the exception into a WebApi error
            WebApiError error = new WebApiError(exception);

            // Log the exception
            this.log.Error(exception.Message, exception);

            // Remove the stack trace information from the error as the client application should not see this.
            error.StackTrace = null;

            WebApiServiceException webApiServiceException = exception as WebApiServiceException;

            if (webApiServiceException != null)
            {
                // Is this a validation exception?
                WebApiServiceValidationException webApiServiceValidationException = webApiServiceException as WebApiServiceValidationException;
                ModelStateDictionary modelState = webApiServiceValidationException != null ? webApiServiceValidationException.ModelState : null;

                // Return the WebApi error in the service response
                error.Message = exception.Message;
                error.ErrorCode = webApiServiceException.ErrorCode;
                error.ValidationErrors = MapValidationErrors(modelState).ToList();

                // Return the specified status code
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(webApiServiceException.HttpStatusCode, error);
            }
            else
            {
                // Unknown exception, so return an internal server error
                error.ErrorCode = WebApiErrorCode.UnknownError;
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Maps the validation errors from modelStateDictionary into the WebApi error representation
        /// </summary>
        /// <param name="modelStateDictionary">State of the model.</param>
        /// <returns>The list of validation errors, empty list if no errors</returns>
        private static IEnumerable<ValidationError> MapValidationErrors(ModelStateDictionary modelStateDictionary)
        {
            if (modelStateDictionary != null)
            {
                foreach (string key in modelStateDictionary.Keys)
                {
                    ModelErrorCollection errors = modelStateDictionary[key].Errors;

                    if (errors.Any())
                    {
                        yield return new ValidationError
                        {
                            Key = key,
                            Errors = errors.Select(error => error.ErrorMessage).ToList()
                        };
                    }
                }
            }
        }
    }
}
