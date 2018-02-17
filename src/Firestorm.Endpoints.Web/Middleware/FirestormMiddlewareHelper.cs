using System;
using System.Threading.Tasks;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Web
{
    public class FirestormMiddlewareHelper
    {
        private readonly FirestormConfiguration _configuration;
        private readonly string _resourcePath;
        private readonly RequestReader _reader;
        private readonly ResponseBuilder _builder;
        private readonly ResponseWriter _writer;

        public FirestormMiddlewareHelper(FirestormConfiguration configuration, IHttpRequestHandler requestHandler)
        {
            _configuration = configuration;

            _resourcePath = requestHandler.ResourcePath;

            _reader = new RequestReader(requestHandler, _configuration.EndpointConfiguration);

            var response = new Response(requestHandler.ResourcePath);

            var modifiers = new DefaultResponseModifiers(configuration.EndpointConfiguration.ResponseConfiguration);
            _builder = new ResponseBuilder(response, modifiers);

            _writer = new ResponseWriter(requestHandler, response, _configuration.EndpointConfiguration);
        }

        /// <summary>
        /// Finds the endpoint and executes the request onto it.
        /// Handles errors and disposes of the endpoint when completed.
        /// </summary>
        public async Task InvokeAsync(IRestEndpointContext endpointContext)
        {
            try
            {
                IRestEndpoint endpoint = GetEndpoint(endpointContext);

                var invoker = new EndpointExecutor(endpoint, _reader, _builder);
                await invoker.ExecuteAsync();

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
            return StartEndpointUtility.GetEndpointFromPath(_configuration, endpointContext, _resourcePath);
        }
    }
}