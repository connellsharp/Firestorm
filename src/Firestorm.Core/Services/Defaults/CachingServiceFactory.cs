using System;

namespace Firestorm
{
    internal class CachingServiceFactory : IServiceFactory
    {
        private readonly Func<IServiceProvider, object> _implementationFactory;
        private object _value;
        private bool _hasLoaded;

        public CachingServiceFactory(Func<IServiceProvider, object> implementationFactory)
        {
            _implementationFactory = implementationFactory;
        }
        
        public object Get(IServiceProvider serviceProvider)
        {
            if (!_hasLoaded)
            {
                _value = _implementationFactory.Invoke(serviceProvider);
                _hasLoaded = true;
            }

            return _value;
        }
    }
}