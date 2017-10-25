using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Firestorm.Endpoints.Formatting
{
    public class ContentNegotiator
    {
        private readonly IContentWriter _writer;

        public ContentNegotiator(IContentWriter writer)
        {
            _writer = writer;
        }

        public async Task SetResponseBody(object obj)
        {
            if (obj == null)
                return;

            string json = JsonConvert.SerializeObject(obj);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            _writer.SetMimeType("application/json; charset=utf-8");
            _writer.SetLength(bytes.Length);
            await _writer.WriteBytesAsync(bytes);
        }
    }
}
