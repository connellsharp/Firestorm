using System;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Start
{
    public class FirestormMiddlewareHelper
    {
        private readonly FirestormConfiguration _configuration;
        private readonly IHttpRequestHandler _requestHandler;
        private readonly IResponseBuilder _builder;

        public FirestormMiddlewareHelper(FirestormConfiguration configuration, IHttpRequestHandler requestHandler)
        {
            _configuration = configuration;
            _requestHandler = requestHandler;

            _builder = new AggregateResponseBuilder(new DefaultResponseBuilders(configuration.EndpointConfiguration.ResponseConfiguration));
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
                var invoker = new EndpointInvoker(_builder, _requestHandler, endpoint);
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

            var writer = new ResponseWriter(_requestHandler, _builder);
            writer.AddError(errorInfo);
            await writer.WriteAsync();
        }
    }
}