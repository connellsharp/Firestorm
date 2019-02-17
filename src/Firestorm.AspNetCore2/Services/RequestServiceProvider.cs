using System;
using Microsoft.AspNetCore.Http;

namespace Firestorm.AspNetCore2
{
    /// <summary>
    /// Uses the <see cref="IHttpContextAccessor"/> to get request-scoped services when given a singleton-scoped service provider.
    /// </summary>
    internal class RequestServiceProvider : IRequestServiceProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Uses the <see cref="IHttpContextAccessor"/> to get request-scoped services.
        /// </summary>
        public RequestServiceProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor
                ?? throw new ArgumentNullException(nameof(contextAccessor), "IHttpContextAccessor must be registered.");
        }

        public object GetService(Type serviceType)
        {
            return _contextAccessor.HttpContext.RequestServices.GetService(serviceType);
        }
    }
}