using Firestorm.Core;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Start;

namespace Firestorm.Tests.Endpoints.Models
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