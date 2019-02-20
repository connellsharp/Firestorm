using System;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Endpoints.Responses;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;

namespace Firestorm.Endpoints.Executors
{
    /// <summary>
    /// Executes the request from the given <see cref="IHttpRequestHandler"/> onto the given <see cref="IRestEndpoint"/>
    /// and builds the response using the <see cref="ResponseBuilder"/>.
    /// </summary>
    internal class EndpointExecutor : IExecutor
    {
        public EndpointExecutor(IRestEndpoint endpoint)
        {
            Endpoint = endpoint;
        }

        public IRestEndpoint Endpoint { get; }

        public async Task<object> ExecuteAsync(IRequestReader reader, ResponseBuilder response)
        {
            switch (reader.RequestMethod)
            {
                case "GET":
                    return await ExecuteGetAsync(reader, response);

                case "OPTIONS":
                    return await ExecuteOptionsAsync(reader, response);

                case "POST":
                case "PUT":
                case "PATCH":
                case "DELETE":
                    return await ExecuteModifyAsync(reader, response);

                default:
                    response.SetStatusCode(HttpStatusCode.MethodNotAllowed);
                    return null;
            }
        }

        private async Task<ResourceBody> ExecuteGetAsync(IRequestReader reader, ResponseBuilder response)
        {
            ResourceBody resourceBody = await Endpoint.GetAsync(reader.GetQuery());

            response.AddResource(resourceBody);

            return resourceBody;
        }

        private async Task<Options> ExecuteOptionsAsync(IRequestReader reader, ResponseBuilder response)
        {
            Options options = await Endpoint.OptionsAsync();

            response.AddOptions(options);

            return options;
        }

        private async Task<Feedback> ExecuteModifyAsync(IRequestReader reader, ResponseBuilder response)
        {
            var method = (UnsafeMethod) Enum.Parse(typeof(UnsafeMethod), reader.RequestMethod, true);
            ResourceBody requestBody = reader.GetRequestBody();
            Feedback feedback = await Endpoint.CommandAsync(method, requestBody);

            response.AddFeedback(feedback);

            return feedback;
        }
    }
}