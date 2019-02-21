using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints.Tests.Stubs
{
    internal class SingletonStartResourceFactory : IStartResourceFactory
    {
        private readonly IRestResource _singletonResource;

        public SingletonStartResourceFactory(IRestResource singletonResource)
        {
            _singletonResource = singletonResource;
        }

        public void Initialize()
        {
            // No initialisation required
        }

        public IRestResource GetStartResource(IRequestContext endpointContext)
        {
            return _singletonResource;
        }
    }
}