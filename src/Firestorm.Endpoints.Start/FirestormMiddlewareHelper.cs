using System;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;

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
        
        /// <summary>
        /// Finds the endpoint and invokes the request onto it.
        /// Handles errors and disposes of the endpoint when completed.
        /// </summary>
        public async Task InvokeAsync(IRestEndpointContext endpointContext)
        {
            try
            {
                IRestEndpoint endpoint = GetEndpoint(endpointContext);
                var invoker = new EndpointInvoker(_configuration.EndpointConfiguration, _requestHandler, endpoint);
                await invoker.InvokeAsync();
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

        private async Task InvokeError(Exception ex)
        {
            var errorInfo = new ExceptionErrorInfo(ex);

            _requestHandler.SetStatusCode((HttpStatusCode) errorInfo.ErrorStatus);

            RestEndpointConfiguration endpointConfig = _configuration.EndpointConfiguration;
            object responseBody = endpointConfig.ResponseContentGenerator.GetFromError(errorInfo, endpointConfig.ShowDeveloperErrors);
            await _requestHandler.SetResponseBody(responseBody);
        }
    }
}