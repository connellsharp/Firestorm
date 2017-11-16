using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Extensions.AspNetCore
{
    /// <summary>
    /// Uses the <see cref="IHttpContextAccessor"/> to get request-scoped services when given a singleton-scoped service provider.
    /// </summary>
    internal class RequestServiceProvider : IServiceProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Uses the <see cref="IHttpContextAccessor"/> to get request-scoped services from the given <see cref="singletonServiceProvider"/>.
        /// </summary>
        public RequestServiceProvider(IServiceProvider singletonServiceProvider)
        {
            _contextAccessor = singletonServiceProvider.GetService<IHttpContextAccessor>();
        }

        public object GetService(Type serviceType)
        {
            return _contextAccessor.HttpContext.RequestServices.GetService(serviceType);
        }
    }
}