using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Firestorm.Client.Content
{
    internal class JsonContentSerializer : IContentSerializer
    {
        private readonly JsonSerializer _jsonSerializer;

        public JsonContentSerializer()
        {
            _jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                // would be better to use the NamingConvention stuff in Endpoints.Formatting. Perhaps move that to Core.Web ?
                ContractResolver = new DefaultContractResolver() { NamingStrategy = new SnakeCaseNamingStrategy() },
                Converters = { new RestItemDataConverter() }
            });
        }

        public async Task<T> DeserializeAsync<T>(HttpResponseMessage response)
        {
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            {
                if (response.IsSuccessStatusCode)
                    return DeserializeFromStream<T>(stream);

                var errorData = DeserializeFromStream<ExceptionResponse>(stream);
                throw new ClientRestApiException(response.StatusCode, errorData);
            }
        }

        private T DeserializeFromStream<T>(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {                
                return _jsonSerializer.Deserialize<T>(jsonReader);
            }
        }

        public StringContent SerializeItemToContent(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                _jsonSerializer.Serialize(stringWriter, obj);
                return new StringContent(stringWriter.ToString(), Encoding.UTF8, "application/json");
            }
        }
    }
}