using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Testing.Http
{
    public class IntegratedStartResourceFactory : IStartResourceFactory
    {
        public void Initialize()
        {
            // Not needed in tests
        }

        public IRestResource GetStartResource(IRequestContext requestContext)
        {
            return new IntegratedRestDirectory(requestContext);
        }
    }
}