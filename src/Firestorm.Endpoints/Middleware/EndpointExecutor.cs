using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Responses;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// Executes the request from the given <see cref="IHttpRequestHandler"/> onto the given <see cref="IRestEndpoint"/>
    /// and builds the response using the <see cref="ResponseBuilder"/>.
    /// </summary>
    internal class EndpointExecutor
    {
        private readonly IRestEndpoint _endpoint;
        private readonly IRequestReader _requestReader;
        private readonly ResponseBuilder _responseBuilder;
        private readonly ResponseConfiguration _configuration;

        public EndpointExecutor(IRestEndpoint endpoint, IRequestReader requestReader,
                                ResponseBuilder responseBuilder, ResponseConfiguration configuration)
        {
            _endpoint = endpoint;
            _requestReader = requestReader;
            _responseBuilder = responseBuilder;
            _configuration = configuration;
        }

        public Task ExecuteAsync()
        {
            switch (_requestReader.RequestMethod)
            {
                case "GET":
                    return ExecuteGetAsync();

                case "OPTIONS":
                    return ExecuteOptionsAsync();

                case "POST":
                case "PUT":
                case "PATCH":
                    return ExecuteModifyAsync();

                case "DELETE":
                    return ExecuteCommandAsync();

                default:
                    _responseBuilder.SetStatusCode(HttpStatusCode.MethodNotAllowed);
                    return Task.FromResult(false);
            }
        }

        private async Task ExecuteGetAsync()
        {
            if (!_endpoint.EvaluatePreconditions(_requestReader.GetPreconditions()))
            {
                _responseBuilder.SetStatusCode(HttpStatusCode.NotModified);
                return;
            }

            ResourceBody resourceBody = await _endpoint.GetAsync(_requestReader.GetQuery());

            _responseBuilder.AddResource(resourceBody);
        }

        private async Task ExecuteOptionsAsync()
        {
            Options options = await _endpoint.OptionsAsync();

            _responseBuilder.AddOptions(options);
        }

        private async Task ExecuteModifyAsync()
        {
            var feedback = await ExecuteCommandAsync();
            await ExecuteReturnAsync(feedback);
        }

        private async Task<Feedback> ExecuteCommandAsync()
        {
            if (!_endpoint.EvaluatePreconditions(_requestReader.GetPreconditions()))
            {
                _responseBuilder.SetStatusCode(HttpStatusCode.PreconditionFailed);
                return null;
            }

            var method =  (UnsafeMethod)Enum.Parse(typeof(UnsafeMethod), _requestReader.RequestMethod, true);
            ResourceBody requestBody = _requestReader.GetRequestBody();
            Feedback feedback = await _endpoint.CommandAsync(method, requestBody);

            _responseBuilder.AddFeedback(feedback);

            return feedback;
        }

        private async Task ExecuteReturnAsync(Feedback feedback)
        {
            // TODO refactor. try include in strategies?
            if (!_configuration.ResourceOnSuccessfulCommand)
                return;
            
            IRestEndpoint endpoint = _endpoint;

            if (feedback is AcknowledgmentFeedback ackFeedback
                && ackFeedback.Acknowledgment is CreatedItemAcknowledgment created)
            {
                endpoint = _endpoint.Next(new RawNextPath(created.NewIdentifier.ToString()));
                Debug.Assert(endpoint != null);
            }

            ResourceBody resourceBody = await endpoint.GetAsync(_requestReader.GetQuery());
            _responseBuilder.AddResource(resourceBody);
        }
    }
}