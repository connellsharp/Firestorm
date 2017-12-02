using Firestorm.Endpoints;
using Firestorm.Endpoints.Start;

namespace Firestorm.Tests.Unit.Endpoints.Stubs
{
    internal class SingletonStartResourceFactory : IStartResourceFactory
    {
        private readonly IRestResource _singletonResource;

        public SingletonStartResourceFactory(IRestResource singletonResource)
        {
            _singletonResource = singletonResource;
        }

        public IRestResource GetStartResource(IRestEndpointContext endpointContext)
        {
            return _singletonResource;
        }

        public void Initialize()
        {
        }
    }
}