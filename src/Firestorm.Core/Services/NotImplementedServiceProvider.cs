using System;

namespace Firestorm.Defaults
{
    public class NotImplementedServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            throw new NotImplementedException("Request service provider is not implemented.");
        }
    }
}