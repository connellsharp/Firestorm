using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Firestorm.Client
{
    public abstract class RestClientResourceBase : IRestResource
    {
        internal RestClientResourceBase(IHttpClientCreator httpClientCreator, string path)
        {
            HttpClientCreator = httpClientCreator;
            Path = path;
        }

        internal IHttpClientCreator HttpClientCreator { get; }

        protected string Path { get; }

        protected static async Task<T> DeserializeAsync<T>(HttpResponseMessage response)
        {
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            {
                if (response.IsSuccessStatusCode)
                    return DeserializeFromStream<T>(stream);

                var errorData = DeserializeFromStream<RestItemData>(stream);
                throw new ClientRestApiException(response.StatusCode, errorData);
            }
        }

        protected static T DeserializeFromStream<T>(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return serializer.Deserialize<T>(jsonReader);
            }
        }

        protected static StringContent SerializeItemToContent(object obj)
        {
            var serializer = new JsonSerializer();

            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, obj);
                return new StringContent(stringWriter.ToString(), Encoding.UTF8, "application/json");
            }
        }

        protected static async Task<Acknowledgment> EnsureSuccessAsync(HttpResponseMessage response)
        {
            var serializer = new JsonSerializer();

            using (Stream stream = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var model = serializer.Deserialize<SuccessOrFailModel>(jsonReader);

                return EnsureSuccess(model, response.StatusCode);
            }
        }

        private static Acknowledgment EnsureSuccess(SuccessOrFailModel model, HttpStatusCode responseStatusCode)
        {
            if (!model.Success)
            {
                if (IsSuccessStatusCode(responseStatusCode))
                    throw new StatusBodyMismatchException();

                throw new UnsuccessfulRestException((ErrorStatus)responseStatusCode, model.ErrorCode, model.ErrorDescription);
            }

            if (!IsSuccessStatusCode(responseStatusCode))
                throw new StatusBodyMismatchException();

            switch (responseStatusCode)
            {
                case HttpStatusCode.OK:
                    return new Acknowledgment();

                case HttpStatusCode.Created:
                    if (model.NewIdentifier == null || string.Empty.Equals(model.NewIdentifier) || 0.Equals(model.NewIdentifier))
                        throw new InvalidOperationException("REST API did not return a new identifier or an error after creating a new item.");

                    return new CreatedItemAcknowledgment(model.NewIdentifier);
            }

            throw new UnknownSuccessStatusCodeException();
        }

        private static bool IsSuccessStatusCode(HttpStatusCode responseStatusCode)
        {
            // This is the same functionality as response.IsSuccessStatusCode.
            return responseStatusCode >= HttpStatusCode.OK && responseStatusCode <= (HttpStatusCode) 299;
        }
    }
}