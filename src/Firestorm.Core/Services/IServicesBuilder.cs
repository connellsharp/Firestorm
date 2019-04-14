using System;

namespace Firestorm
{
    /// <summary>
    /// An interface that can be extended to inject Firestorm services.
    /// </summary>
    public interface IServicesBuilder
    {
        IServicesBuilder Add<TService>(Func<IServiceProvider, TService> implementationFactory) 
            where TService : class;
        
        IServicesBuilder Add(Type serviceType, Type implementationType);
        
        IServicesBuilder Add<TService>(TService implementationInstance)
            where TService : class;
    }
}