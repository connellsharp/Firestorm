using System;
using Firestorm.Host;

namespace Firestorm.Owin
{
    public class OwinServicesBuilder : IFirestormServicesBuilder
    {
        // TODO internal IOC container
        
        public IFirestormServicesBuilder Add<TService>(Func<IFirestormServiceProvider, TService> implementationFactory) 
            where TService : class
        {
            throw new NotImplementedException();
        }

        public IFirestormServicesBuilder Add(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IFirestormServicesBuilder Add<TService>(TService implementationInstance) 
            where TService : class
        {
            throw new NotImplementedException();
        }

        public IFirestormServicesBuilder Add<TAbstraction, TImplementation>() 
            where TAbstraction : class where TImplementation : class, TAbstraction
        {
            throw new NotImplementedException();
        }
        
        public IFirestormServiceProvider Build()
        {
            throw new NotImplementedException();
        }
    }
}