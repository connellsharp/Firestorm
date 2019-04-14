using System;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.AspNetCore2
{
    internal class AspNetCoreServicesBuilder : IServicesBuilder
    {
        private readonly IServiceCollection _services;

        public AspNetCoreServicesBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IServicesBuilder Add<TService>(Func<IServiceProvider, TService> implementationFactory) 
            where TService : class
        {
            _services.AddSingleton<TService>(implementationFactory);
            return this;
        }

        public IServicesBuilder Add(Type serviceType, Type implementationType)
        {
            _services.AddSingleton(serviceType, implementationType);
            return this;
        }

        public IServicesBuilder Add<TService>(TService implementationInstance) 
            where TService : class
        {
            _services.AddSingleton<TService>(implementationInstance);
            return this;
        }
    }
}