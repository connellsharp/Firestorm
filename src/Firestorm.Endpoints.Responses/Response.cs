using System.Collections.Generic;
using System.Net;

namespace Firestorm.Endpoints.Responses
{
    public class Response
    {
        public Response(string resourcePath)
        {
            ResourcePath = resourcePath;
        }

        public string ResourcePath { get; private set; }

        public HttpStatusCode StatusCode { get; set; }

        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public object ResourceBody { get; set; }

        public RestItemData ExtraBody { get; } = new RestItemData();
    }
}
