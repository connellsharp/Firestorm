using System.IO;
using Firestorm.Rest.Web;
using Firestorm.Endpoints.Formatting.Json;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints.Formatting
{
    public class ContentParser
    {
        private readonly IContentReader _reader;
        private readonly INamingConventionSwitcher _switcher;

        public ContentParser(IContentReader reader, INamingConventionSwitcher switcher)
        {
            _reader = reader;
            _switcher = switcher;
        }

        public ResourceBody GetRequestBody()
        {
            string mime = _reader.GetMimeType();

            //if (mime != null && !mime.Contains("json"))
            //    throw new ContentTypeNotSupportedException();

            Stream stream = _reader.GetContentStream();

            if (stream == null)
                return null;

            var bodyReader = new ResourceBodyReader(_switcher);
            return bodyReader.ReadResourceStream(stream);
        }

        private class ContentTypeNotSupportedException : RestApiException
        {
            public ContentTypeNotSupportedException()
                : base(ErrorStatus.BadRequest, "Request content mime type is not supported.")
            { }
        }
    }
}