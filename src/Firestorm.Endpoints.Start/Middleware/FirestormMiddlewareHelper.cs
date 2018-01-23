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
        private readonly ResponseBuilder _builder;
        private readonly ResponseWriter _writer;

        public FirestormMiddlewareHelper(FirestormConfiguration configuration, IHttpRequestHandler requestHandler)
        {
            _configuration = configuration;
            _requestHandler = requestHandler;
            
            var response = new Response(requestHandler.ResourcePath);

            var modifiers = new DefaultResponseModifiers(configuration.EndpointConfiguration.ResponseConfiguration);
            _builder = new ResponseBuilder(response, modifiers);

            _writer = new ResponseWriter(_requestHandler, response);
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

                var invoker = new EndpointInvoker(_requestHandler, endpoint, _builder);
                await invoker.InvokeAsync();

                await _writer.WriteAsync();
            }
            catch (Exception ex)
            {
                var errorInfo = new ExceptionErrorInfo(ex);
                _builder.AddError(errorInfo);

                await _writer.WriteAsync();
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
    }
}