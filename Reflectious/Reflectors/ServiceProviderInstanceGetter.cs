using System;

namespace Firestorm
{
    public class ServiceProviderInstanceGetter : IInstanceGetter
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderInstanceGetter(IServiceProvider serviceProvider, Type type)
        {
            _serviceProvider = serviceProvider;
            Type = type;
        }

        public Type Type { get; }
        
        public object GetInstance()
        {
            return _serviceProvider.GetService(Type);
        }
    }
}