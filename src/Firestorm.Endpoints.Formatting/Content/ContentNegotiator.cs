using System.Text;
using System.Threading.Tasks;
using Firestorm.Endpoints.Formatting.Json;
using Firestorm.Host.Infrastructure;
using Newtonsoft.Json;

namespace Firestorm.Endpoints.Formatting
{
    public class ContentNegotiator
    {
        private readonly IContentAccepts _accepts;
        private readonly IContentWriter _writer;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public ContentNegotiator(IContentAccepts accepts, IContentWriter writer, INamingConventionSwitcher namingConventionSwitcher)
        {
            _accepts = accepts;
            _writer = writer;

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new NameSwitcherContractResolver(namingConventionSwitcher),
                //Converters = { new RestItemDataJsonConverter(namingConventionSwitcher) }
            };
        }

        public async Task SetResponseBodyAsync(object obj)
        {
            if (obj == null)
                return;

            _writer.SetMimeType("application/json; charset=utf-8");

            string json = JsonConvert.SerializeObject(obj, _jsonSerializerSettings);

            byte[] bytes = Encoding.UTF8.GetBytes(json);

            _writer.SetLength(bytes.Length);
            await _writer.WriteBytesAsync(bytes);
        }
    }
}
