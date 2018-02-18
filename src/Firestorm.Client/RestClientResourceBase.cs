using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Firestorm.Client
{
    public abstract class RestClientResourceBase : IRestResource
    {
        internal RestClientResourceBase(IHttpClientCreator httpClientCreator, string path)
        {
            HttpClientCreator = httpClientCreator;
            Path = path;
            Serializer = new JsonContentSerializer();
        }

        internal IHttpClientCreator HttpClientCreator { get; }

        protected string Path { get; }

        internal IContentSerializer Serializer { get; }

        protected async Task<Acknowledgment> EnsureSuccessAsync(HttpResponseMessage response)
        {
            var model = await Serializer.DeserializeAsync<SuccessOrFailModel>(response);
            return EnsureSuccess(model, response.StatusCode);
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