using System.IO;
using System.Text;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints.Tests.Start.Readers
{
    internal class MockJsonReader : IContentReader
    {
        private readonly string _json;

        public MockJsonReader(string json)
        {
            _json = json;
        }

        public Stream GetContentStream()
        {
            return new MemoryStream(Encoding.Default.GetBytes(_json));
        }

        public string GetMimeType()
        {
            return "application/json";
        }
    }
}