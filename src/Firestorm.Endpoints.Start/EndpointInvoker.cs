using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Start
{
    /// <summary>
    /// Invokes the request from the given <see cref="IHttpRequestHandler"/> onto the given <see cref="IRestEndpoint"/>
    /// </summary>
    internal class EndpointInvoker
    {
        private readonly RestEndpointConfiguration _configuration;
        private readonly IHttpRequestHandler _requestHandler;
        private readonly IRestEndpoint _endpoint;

        public EndpointInvoker(RestEndpointConfiguration configuration, IHttpRequestHandler requestHandler, IRestEndpoint endpoint)
        {
            _configuration = configuration;
            _requestHandler = requestHandler;
            _endpoint = endpoint;
        }

        public async Task InvokeAsync()
        {
            switch (_requestHandler.RequestMethod)
            {
                case "GET":
                    await InvokeGetAsync();
                    return;

                case "OPTIONS":
                    await InvokeOptionsAsync();
                    return;

                case "POST":
                case "PUT":
                case "PATCH":
                case "DELETE":
                    await InvokeUnsafeAsync();
                    return;

                default:
                    _requestHandler.SetStatusCode(HttpStatusCode.MethodNotAllowed);
                    return;
            }
        }

        private async Task InvokeGetAsync()
        {
            if (!_endpoint.EvaluatePreconditions(_requestHandler.GetPreconditions()))
            {
                _requestHandler.SetStatusCode(HttpStatusCode.NotModified);
                return;
            }

            ResourceBody resourceBody = await _endpoint.GetAsync();

            if (resourceBody is IPagedResourceBody pagedResourceBody)
            {
                var setter = new LinkHeaderBuilder();
                setter.AddDetails(pagedResourceBody.PageLinks);
                setter.SetHeaders(_requestHandler);
            }

            object resBody = _configuration.ResponseContentGenerator.GetFromResource(resourceBody);
            await _requestHandler.SetResponseBody(resBody);
        }

        private async Task InvokeOptionsAsync()
        {
            Options options = await _endpoint.OptionsAsync();
            object optionsBody = _configuration.ResponseContentGenerator.GetFromOptions(options);
            await _requestHandler.SetResponseBody(optionsBody);
        }

        private async Task InvokeUnsafeAsync()
        {
            if (!_endpoint.EvaluatePreconditions(_requestHandler.GetPreconditions()))
            {
                _requestHandler.SetStatusCode(HttpStatusCode.PreconditionFailed);
                return;
            }

            var method = (UnsafeMethod)Enum.Parse(typeof(UnsafeMethod), _requestHandler.RequestMethod, true);
            ResourceBody postBody = _requestHandler.GetRequestBodyObject();

            Feedback feedback = await _endpoint.UnsafeAsync(method, postBody);

            await WriteFeedbackToResponse(feedback);
        }

        private async Task WriteFeedbackToResponse(Feedback feedback)
        {
            var converter = new FeedbackToResponseConverter(feedback, _configuration);

            HttpStatusCode status = converter.GetStatusCode();
            _requestHandler.SetStatusCode(status);

            if (status == HttpStatusCode.Created)
            {
                object newIdentifier = converter.GetNewIdentifier();
                Debug.Assert(newIdentifier != null, "Status code 201 should mean there is a new identifier.");
                string newUrl = string.Format("{0}/{1}", _requestHandler.ResourcePath.TrimEnd('/'), newIdentifier);
                _requestHandler.SetResponseHeader("Location", newUrl);
            }

            object feedbackBody = converter.GetBody();
            await _requestHandler.SetResponseBody(feedbackBody);
        }
    }
}