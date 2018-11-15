using Firestorm.Host;

namespace Firestorm.Tests.Unit.Endpoints.Stubs
{
    internal class SingletonStartResourceFactory : IStartResourceFactory
    {
        private readonly IRestResource _singletonResource;

        public SingletonStartResourceFactory(IRestResource singletonResource)
        {
            _singletonResource = singletonResource;
        }

        public IRestResource GetStartResource(IRequestContext endpointContext)
        {
            return _singletonResource;
        }

        public void Initialize()
        {
        }
    }
}