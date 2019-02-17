using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Formatting;
using Firestorm.Host.Infrastructure;
using Firestorm.Rest.Web;

namespace Firestorm.Endpoints
{
    internal class RequestReader : IRequestReader
    {
        private readonly IHttpRequestReader _httpReader;
        private readonly INamingConventionSwitcher _nameSwitcher;
        private readonly IQueryCreator _queryCreator;
        
        public RequestReader(IHttpRequestReader httpReader, INamingConventionSwitcher nameSwitcher, IQueryCreator queryCreator)
        {
            _httpReader = httpReader;
            _nameSwitcher = nameSwitcher;
            _queryCreator = new NameSwitcherQueryCreator(queryCreator, nameSwitcher);
        }

        public string RequestMethod
        {
            get { return _httpReader.RequestMethod; }
        }

        public IPreconditions GetPreconditions() => _httpReader.GetPreconditions();

        public ResourceBody GetRequestBody()
        {
            var parser = new ContentParser(_httpReader.GetContentReader(), _nameSwitcher);
            return parser.GetRequestBody();
        }

        public IRestCollectionQuery GetQuery()
        {
            string queryString = _httpReader.GetQueryString();

            if (string.IsNullOrEmpty(queryString))
                return null;

            return _queryCreator.Create(queryString);
        }
    }
}