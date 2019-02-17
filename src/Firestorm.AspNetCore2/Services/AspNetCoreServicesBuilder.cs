using System;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.AspNetCore2
{
    internal class AspNetCoreServicesBuilder : IFirestormServicesBuilder
    {
        private readonly IServiceCollection _services;

        public AspNetCoreServicesBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IFirestormServicesBuilder Add<TService>(Func<IServiceProvider, TService> implementationFactory) 
            where TService : class
        {
            _services.AddSingleton<TService>(implementationFactory);
            return this;
        }

        public IFirestormServicesBuilder Add(Type serviceType, Type implementationType)
        {
            _services.AddSingleton(serviceType, implementationType);
            return this;
        }

        public IFirestormServicesBuilder Add<TService>(TService implementationInstance) 
            where TService : class
        {
            _services.AddSingleton<TService>(implementationInstance);
            return this;
        }
    }
}