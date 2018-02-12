using Firestorm.Core.Web;
using Firestorm.Endpoints.Formatting;
using Firestorm.Endpoints.Preconditions;
using Firestorm.Endpoints.Query;

namespace Firestorm.Endpoints.Start
{
    internal class RequestReader : IRequestReader
    {
        private readonly IHttpRequestReader _httpReader;
        private readonly RestEndpointConfiguration _endpointConfiguration;

        public RequestReader(IHttpRequestReader httpReader, RestEndpointConfiguration endpointConfiguration)
        {
            _httpReader = httpReader;
            _endpointConfiguration = endpointConfiguration;
        }

        public string RequestMethod => _httpReader.RequestMethod;

        public IPreconditions GetPreconditions() => _httpReader.GetPreconditions();

        public ResourceBody GetRequestBody()
        {
            var parser = new ContentParser(_httpReader.GetContentReader(), _endpointConfiguration.NamingConventionSwitcher);
            return parser.GetRequestBody();
        }

        public IRestCollectionQuery GetQuery()
        {
            return new QueryStringCollectionQuery(_endpointConfiguration.QueryStringConfiguration, _httpReader.GetQueryString());
        }
    }
}