namespace BeerAppreciation.Core.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;
    using global::Castle.MicroKernel;

    /// <summary>
    /// Castle windsor implementation of WebApi IDependencyResolver interface
    /// </summary>
    public sealed class WindsorDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        /// <summary>
        /// The Castle Windsor kernel
        /// </summary>
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorDependencyResolver"/> class.
        /// </summary>
        /// <param name="kernel">The Castle Windsor kernel.</param>
        public WindsorDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        /// <summary>
        /// Resolves an interface to a concrete implementation
        /// </summary>
        /// <param name="serviceType">The type to resolve</param>
        /// <returns>A concrete implementation</returns>
        public object GetService(Type serviceType)
        {
            return this.kernel.HasComponent(serviceType) ? this.kernel.Resolve(serviceType) : null;
        }

        /// <summary>
        /// Resolves an interface to concrete implementations
        /// </summary>
        /// <param name="serviceType">The type to resolve</param>
        /// <returns>A list of concrete implementation</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.ResolveAll(serviceType) as IEnumerable<object>;
        }

        /// <summary>
        /// Begins the scope.
        /// </summary>
        /// <returns>An IDependencyScope</returns>
        public IDependencyScope BeginScope()
        {
            // Not interested in dealing with child scopes so just return 'this'
            return this;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // This must do nothing as we're not dealing with child scopes
            GC.SuppressFinalize(this);
        }
    }
}