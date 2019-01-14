using System;
using System.Threading.Tasks;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Responses;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints
{
    public class EndpointsRequestInvoker : IRequestInvoker
    {
        private readonly IStartResourceFactory _startResourceFactory;
        private readonly RestEndpointConfiguration _configuration;

        public EndpointsRequestInvoker(IStartResourceFactory startResourceFactory, RestEndpointConfiguration configuration)
        {
            _startResourceFactory = startResourceFactory;
            _configuration = configuration;
        }
        
        public void Initialize()
        {
            _startResourceFactory.Initialize();
        }

        /// <summary>
        /// Finds the endpoint and executes the request onto it.
        /// Handles errors and disposes of the endpoint when completed.
        /// </summary>
        public async Task InvokeAsync(IHttpRequestReader requestReader, IHttpRequestResponder responder, IRequestContext context)
        {            
            var reader = new RequestReader(requestReader, _configuration);

            var response = new Response(requestReader.ResourcePath);

            var modifiers = new DefaultResponseModifiers(_configuration.Response);
            var builder = new ResponseBuilder(response, modifiers);

            var writer = new ResponseWriter(responder, response, _configuration);
            
            try
            {
                var navigator = new EndpointNavigator(context, _startResourceFactory, _configuration);
                IRestEndpoint endpoint =  navigator.GetEndpointFromPath(requestReader.ResourcePath);

                var invoker = new EndpointExecutor(endpoint, reader, builder, _configuration.Response);
                await invoker.ExecuteAsync();

                await writer.WriteAsync();
            }
            catch (Exception ex)
            {
                var errorInfo = new ExceptionErrorInfo(ex);
                builder.AddError(errorInfo);

                await writer.WriteAsync();
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}