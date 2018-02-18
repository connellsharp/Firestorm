using System;
using System.Linq;
using System.Net.Http;

namespace Firestorm.Client
{
    public class RestClient : IHttpClientCreator
    {
        private readonly string _baseAddress;

        public RestClient(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public RestClientCollection RequestCollection(params object[] directories)
        {
            string path = CombinePaths(directories);
            return new RestClientCollection(this, path);
        }

        public RestClientItem RequestItem(params object[] directories)
        {
            string path = CombinePaths(directories);
            return new RestClientItem(this, path);
        }

        public RestClientScalar RequestScalar(params object[] directories)
        {
            string path = CombinePaths(directories);
            return new RestClientScalar(this, path);
        }

        private static string CombinePaths(object[] directories)
        {
            return string.Join("/", directories.Select(dir => dir.ToString()));
        }

        public HttpClient Create()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseAddress);
            return client;
        }
    }
}
