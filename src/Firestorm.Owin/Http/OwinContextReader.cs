using Firestorm.Host.Infrastructure;
using Microsoft.Owin;

namespace Firestorm.Owin.Http
{
    internal class OwinContextReader : IHttpRequestReader
    {
        private readonly IOwinContext _owinContext;

        public OwinContextReader(IOwinContext owinContext)
        {
            _owinContext = owinContext;
        }

        public string RequestMethod => _owinContext.Request.Method;

        public string ResourcePath => _owinContext.Request.Path.Value;

        public IPreconditions GetPreconditions()
        {
            return new OwinPreconditions(_owinContext);
        }

        public IContentReader GetContentReader()
        {
            return new OwinContentReader(_owinContext);
        }

        public string GetQueryString()
        {
            return _owinContext.Request.QueryString.Value;
        }
    }
}