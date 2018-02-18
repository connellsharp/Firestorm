using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Firestorm.Client
{
    public class RestClientScalar : RestClientResourceBase, IRestScalar
    {
        internal RestClientScalar(IHttpClientCreator httpClientCreator, string path)
            : base(httpClientCreator, path)
        {
        }

        public async Task<object> GetAsync()
        {
            using (HttpClient client = HttpClientCreator.Create())
            {
                HttpResponseMessage response = await client.GetAsync(Path);
                return await Serializer.DeserializeAsync<object>(response);
            }
        }

        public async Task<Acknowledgment> EditAsync(object value)
        {
            using (HttpClient client = HttpClientCreator.Create())
            {
                HttpResponseMessage response = await client.PutAsync(Path, Serializer.SerializeItemToContent(value));
                return await EnsureSuccessAsync(response);
            }
        }
    }
}