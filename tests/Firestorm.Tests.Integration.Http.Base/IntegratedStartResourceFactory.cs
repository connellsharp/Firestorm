using Firestorm.Core;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Start;

namespace Firestorm.Tests.Integration.Http.Base
{
    public class IntegratedStartResourceFactory : IStartResourceFactory
    {
        public IRestResource GetStartResource(IRestEndpointContext endpointContext)
        {
            return new IntegratedRestDirectory(endpointContext);
        }

        public void Initialize()
        {
        }
    }
}