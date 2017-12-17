using System.IO;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Formatting.Json;

namespace Firestorm.Endpoints.Formatting
{
    public class ContentReader
    {
        public ResourceBody ReadResourceStream(Stream requestBody)
        {
            return ResourceBodyJsonConverter.ReadResourceStream(requestBody);
        }
    }
}