using Firestorm.Host;

namespace Firestorm.Endpoints.Web
{
    internal class RestEndpointContext : IRestEndpointContext
    {
        public RestEndpointContext(IRequestContext endpointContext, RestEndpointConfiguration configuration)
        {
            Request = endpointContext;
            Configuration = configuration;
        }

        public RestEndpointConfiguration Configuration { get; }

        public IRequestContext Request { get; }

        public void Dispose()
        {
            Request.Dispose();
        }
    }
}