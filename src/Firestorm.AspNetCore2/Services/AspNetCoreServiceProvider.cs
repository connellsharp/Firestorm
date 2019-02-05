using System;

namespace Firestorm.AspNetCore2
{
    internal class AspNetCoreServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public AspNetCoreServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }
    }
}