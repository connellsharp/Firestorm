using System.IO;
using Firestorm.Host.Infrastructure;
using Microsoft.Owin;

namespace Firestorm.Owin.Http
{
    internal class OwinContentReader : IContentReader
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