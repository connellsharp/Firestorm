using System;

namespace Firestorm.AspNetCore2
{
    internal class AspNetCoreServiceProvider : IFirestormServiceProvider
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

        public IServiceProvider GetRequestServiceProvider()
        {
            return new RequestServiceProvider(_serviceProvider);
        }
    }
}