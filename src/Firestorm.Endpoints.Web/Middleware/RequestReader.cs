using Firestorm.Core.Web;
using Firestorm.Endpoints.Formatting;
using Firestorm.Endpoints.Preconditions;

namespace Firestorm.Endpoints.Start
{
    internal class RequestReader
    {
        private readonly IHttpRequestReader _requestReader;
        private readonly INamingConventionSwitcher _nameSwitcher;

        internal RequestReader(IHttpRequestReader requestReader, INamingConventionSwitcher nameSwitcher)
        {
            _requestReader = requestReader;
            _nameSwitcher = nameSwitcher;
        }

        public string RequestMethod => _requestReader.RequestMethod;

        public IPreconditions GetPreconditions() => _requestReader.GetPreconditions();

        public ResourceBody GetRequestBody()
        {
            var parser = new ContentParser(_requestReader.GetContentReader(), _nameSwitcher);
            return parser.GetRequestBody();
        }
    }
}