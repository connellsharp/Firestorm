using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Firestorm.Endpoints.Formatting
{
    public class ContentNegotiator
    {
        private readonly IContentAccepts _accepts;
        private readonly IContentWriter _writer;

        public ContentNegotiator(IContentAccepts accepts, IContentWriter writer)
        {
            _accepts = accepts;
            _writer = writer;
        }

        public async Task SetResponseBodyAsync(object obj)
        {
            if (obj == null)
                return;

            _writer.SetMimeType("application/json; charset=utf-8");

            string json = JsonConvert.SerializeObject(obj);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            _writer.SetLength(bytes.Length);
            await _writer.WriteBytesAsync(bytes);
        }
    }

    public interface IContentAccepts
    { }
}
