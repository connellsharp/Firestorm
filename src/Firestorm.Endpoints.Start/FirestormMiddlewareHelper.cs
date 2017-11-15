using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Responses;
using JetBrains.Annotations;

namespace Firestorm.Endpoints.Start
{
    public class FirestormMiddlewareHelper
    {
        private readonly FirestormConfiguration _configuration;
        private readonly IHttpRequestHandler _requestHandler;

        public FirestormMiddlewareHelper(FirestormConfiguration configuration, IHttpRequestHandler requestHandler)
        {
            _configuration = configuration;
            _requestHandler = requestHandler;
        }

        [UsedImplicitly]
        public async Task InvokeAsync(IRestEndpointContext endpointContext)
        {
            try
            {
                switch (_requestHandler.RequestMethod)
                {
                    case "GET":
                        await InvokeGet(endpointContext);
                        return;

                    case "OPTIONS":
                        await InvokeOptions(endpointContext);
                        return;

                    case "POST":
                    case "PUT":
                    case "PATCH":
                    case "DELETE":
                        await InvokeUnsafe(endpointContext);
                        return;

                    default:
                        _requestHandler.SetStatusCode(HttpStatusCode.MethodNotAllowed);
                        return;
                }
            }
            catch (Exception ex)
            {
                await InvokeError(ex);
            }
            finally
            {
                endpointContext.Dispose();
            }
        }

        private IRestEndpoint GetEndpoint(IRestEndpointContext endpointContext)
        {
            return StartUtilities.GetEndpointFromPath(_configuration.StartResourceFactory, endpointContext, _requestHandler.ResourcePath);
        }

        private async Task InvokeGet(IRestEndpointContext endpointContext)
        {
            IRestEndpoint endpoint = GetEndpoint(endpointContext);

            if (!endpoint.EvaluatePreconditions(_requestHandler.GetPreconditions()))
            {
                _requestHandler.SetStatusCode(HttpStatusCode.NotModified);
                return;
            }

            ResourceBody resourceBody = await endpoint.GetAsync();
            object resBody = _configuration.EndpointConfiguration.ResponseContentGenerator.GetFromResource(resourceBody);
            await _requestHandler.SetResponseBody(resBody);
        }

        private async Task InvokeOptions(IRestEndpointContext endpointContext)
        {
            IRestEndpoint endpoint = GetEndpoint(endpointContext);

            Options options = await endpoint.OptionsAsync();
            object optionsBody = _configuration.EndpointConfiguration.ResponseContentGenerator.GetFromOptions(options);
            await _requestHandler.SetResponseBody(optionsBody);
        }

        private async Task InvokeUnsafe(IRestEndpointContext endpointContext)
        {
            IRestEndpoint endpoint = GetEndpoint(endpointContext);

            if (!endpoint.EvaluatePreconditions(_requestHandler.GetPreconditions()))
            {
                _requestHandler.SetStatusCode(HttpStatusCode.PreconditionFailed);
                return;
            }

            var method = (UnsafeMethod)Enum.Parse(typeof(UnsafeMethod), _requestHandler.RequestMethod, true);
            ResourceBody postBody = _requestHandler.GetRequestBodyObject();

            Feedback feedback = await endpoint.UnsafeAsync(method, postBody);

            await WriteFeedbackToResponse(feedback);
        }

        private async Task InvokeError(Exception ex)
        {
            var errorInfo = new ExceptionErrorInfo(ex);

            _requestHandler.SetStatusCode((HttpStatusCode) errorInfo.ErrorStatus);

            RestEndpointConfiguration endpointConfig = _configuration.EndpointConfiguration;
            object responseBody = endpointConfig.ResponseContentGenerator.GetFromError(errorInfo, endpointConfig.ShowDeveloperErrors);
            await _requestHandler.SetResponseBody(responseBody);
        }

        private async Task WriteFeedbackToResponse(Feedback feedback)
        {
            var converter = new FeedbackToResponseConverter(feedback, _configuration.EndpointConfiguration);

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