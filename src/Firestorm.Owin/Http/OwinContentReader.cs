using System.IO;
using Firestorm.Endpoints.Formatting;
using Microsoft.Owin;

namespace Firestorm.Owin
{
    public class OwinContentReader : IContentReader
    {
        private readonly IOwinContext _owinContext;

        public OwinContentReader(IOwinContext owinContext)
        {
            _owinContext = owinContext;
        }

        public Stream GetContentStream()
        {
            return _owinContext.Request.Body;
        }

        public string GetMimeType()
        {
            return _owinContext.Request.ContentType;
        }
    }
}