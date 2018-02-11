using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Formatting;
using Firestorm.Endpoints.Preconditions;
using Firestorm.Endpoints.Start;
using Microsoft.Owin;

namespace Firestorm.Owin
{
    public class OwinContextHandler : IHttpRequestHandler
    {
        private readonly IOwinContext _owinContext;

        public OwinContextHandler(IOwinContext owinContext)
        {
            _owinContext = owinContext;
        }

        public string RequestMethod => _owinContext.Request.Method;

        public string ResourcePath => _owinContext.Request.Path.Value;

        public void SetStatusCode(HttpStatusCode statusCode)
        {
            _owinContext.Response.StatusCode = (int) statusCode;
        }

        public IPreconditions GetPreconditions()
        {
            return new OwinPreconditions(_owinContext);
        }

        public IContentReader GetContentReader()
        {
            return new OwinContentReader(_owinContext);
        }

        public IContentAccepts GetAcceptHeaders()
        {
            return new OwinContentAccepts();
        }

        public IContentWriter GetContentWriter()
        {
            return new OwinContentWriter(_owinContext);
        }

        public void SetResponseHeader(string key, string value)
        {
            _owinContext.Response.Headers[key] = value;
        }
    }

    public class OwinContentAccepts : IContentAccepts
    { }
}