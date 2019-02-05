using System;

namespace Firestorm
{
    /// <summary>
    /// An interface that can be extended to inject Firestorm services.
    /// </summary>
    public interface IFirestormServicesBuilder
    {
        IFirestormServicesBuilder Add<TService>(Func<IServiceProvider, TService> implementationFactory) 
            where TService : class;
        
        IFirestormServicesBuilder Add(Type serviceType, Type implementationType);
        
        IFirestormServicesBuilder Add<TService>(TService implementationInstance)
            where TService : class;
    }
}