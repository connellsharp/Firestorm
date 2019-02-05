using System;
using Firestorm.Host;
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
            _services.AddSingleton<TService>(sp => implementationFactory(new AspNetCoreServiceProvider(sp)));
            return this;
        }

        public IFirestormServicesBuilder Add(Type serviceType)
        {
            _services.AddSingleton(serviceType);
            return this;
        }

        public IFirestormServicesBuilder Add<TService>(TService implementationInstance) 
            where TService : class
        {
            _services.AddSingleton<TService>(implementationInstance);
            return this;
        }

        public IFirestormServicesBuilder Add<TAbstraction, TImplementation>() 
            where TImplementation : class, TAbstraction
            where TAbstraction : class
        {
            _services.AddSingleton<TAbstraction, TImplementation>();
            return this;
        }
    }
}