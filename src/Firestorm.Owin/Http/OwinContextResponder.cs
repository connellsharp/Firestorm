using System.Net;
using Firestorm.Endpoints.Formatting;
using Firestorm.Endpoints.Web;
using Microsoft.Owin;

namespace Firestorm.Owin
{
    public class OwinContextResponder : IHttpRequestResponder
    {
        private readonly IOwinContext _owinContext;

        public OwinContextResponder(IOwinContext owinContext)
        {
            _owinContext = owinContext;
        }

        public void SetStatusCode(HttpStatusCode statusCode)
        {
            _owinContext.Response.StatusCode = (int) statusCode;
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
}