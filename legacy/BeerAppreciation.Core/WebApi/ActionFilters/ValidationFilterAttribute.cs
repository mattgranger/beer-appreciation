namespace BeerAppreciation.Core.WebApi.ActionFilters
{
    using Exceptions;
    using System;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using System.Web.Http.ModelBinding;

    /// <summary>
    /// Global action filter that is responsible for validating incoming entities using data annotation attributes
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called before the action is executed.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            ModelStateDictionary modelState = actionContext.ModelState;

            if (!modelState.IsValid)
            {
                throw new WebApiServiceValidationException("The incoming request is invalid", modelState);
            }
        }
    }
}
