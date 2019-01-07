using Firestorm.Host;
using Firestorm.Testing.Http;

namespace Firestorm.Tests.Integration.Http.Base
{
    public class IntegratedStartResourceFactory : IStartResourceFactory
    {
        public IRestResource GetStartResource(IRequestContext requestContext)
        {
            return new IntegratedRestDirectory(requestContext);
        }

        public void Initialize()
        {
        }
    }
}