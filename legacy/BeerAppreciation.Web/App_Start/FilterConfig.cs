namespace BeerAppreciation.Web
{
    using System.Web.Http.Filters;
    using System.Web.Mvc;
    using Castle.Windsor;
    using Core.WebApi.ActionFilters;

    /// <summary>
    /// Register the global filters for the webapi actions
    /// </summary>
    public static class FilterConfig
    {
        /// <summary>
        /// Registers the global filters used by WebApi
        /// </summary>
        /// <param name="filters">The filters collection</param>
        /// <param name="container">The IoC container</param>
        public static void RegisterWebApiGlobalFilters(HttpFilterCollection filters, IWindsorContainer container)
        {
            // Register the global action filter for handling web api exceptions
            filters.Add(container.Resolve<WebApiExceptionFilterAttribute>());

            // Register the global action filter for handling Validation of incoming requests
            filters.Add(container.Resolve<ValidationFilterAttribute>());
        }
    }
}
