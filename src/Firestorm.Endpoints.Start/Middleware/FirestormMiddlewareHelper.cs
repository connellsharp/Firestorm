using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Start.Responses;

namespace Firestorm.Endpoints.Start
{
    public class FirestormMiddlewareHelper
    {
        private readonly FirestormConfiguration _configuration;
        private readonly IHttpRequestHandler _requestHandler;
        private readonly List<IResponseBuilder> _builders;

        public FirestormMiddlewareHelper(FirestormConfiguration configuration, IHttpRequestHandler requestHandler)
        {
            _configuration = configuration;
            _requestHandler = requestHandler;

            _builders = new List<IResponseBuilder>
            {
                new PageHeadersResponseBuilder(),
                new FeedbackResponseBuilder(),
                new ResponseBodyBuilder(_configuration.EndpointConfiguration.ResponseContentGenerator, _configuration.EndpointConfiguration.ShowDeveloperErrors)
            };
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
                var invoker = new EndpointInvoker(_builders, _requestHandler, endpoint);
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

            // TODO: to responseBuilder?
            RestEndpointConfiguration endpointConfig = _configuration.EndpointConfiguration;
            object responseBody = endpointConfig.ResponseContentGenerator.GetFromError(errorInfo, endpointConfig.ShowDeveloperErrors);
            await _requestHandler.SetResponseBodyAsync(responseBody);
        }
    }
}