using Firestorm.Core.Web;
using Firestorm.Endpoints.Formatting;
using Firestorm.Endpoints.Formatting.Naming;
using Firestorm.Endpoints.Preconditions;
using Firestorm.Endpoints.Query;

namespace Firestorm.Endpoints.Web
{
    internal class RequestReader : IRequestReader
    {
        private readonly IHttpRequestReader _httpReader;
        private readonly RestEndpointConfiguration _config;

        public RequestReader(IHttpRequestReader httpReader, RestEndpointConfiguration config)
        {
            _httpReader = httpReader;
            _config = config;
        }

        public string RequestMethod => _httpReader.RequestMethod;

        public IPreconditions GetPreconditions() => _httpReader.GetPreconditions();

        public ResourceBody GetRequestBody()
        {
            var parser = new ContentParser(_httpReader.GetContentReader(), _config.NamingConventionSwitcher);
            return parser.GetRequestBody();
        }

        public IRestCollectionQuery GetQuery()
        {
            string queryString = _httpReader.GetQueryString();

            if (string.IsNullOrEmpty(queryString))
                return null;

            var query = new QueryStringCollectionQuery(_config.QueryStringConfiguration, queryString);
            return NameSwitcherUtility.TryWrapQuery(query, _config.NamingConventionSwitcher);
        }
    }
}