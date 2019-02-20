using System;
using System.Threading.Tasks;
using Firestorm.Endpoints.Executors;
using Firestorm.Endpoints.Responses;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints
{
    public class EndpointsRequestInvoker : IRequestInvoker
    {
        private readonly IStartResourceFactory _startResourceFactory;
        private readonly EndpointServices _services;

        public EndpointsRequestInvoker(IStartResourceFactory startResourceFactory, EndpointServices services)
        {
            _startResourceFactory = startResourceFactory;
            _services = services;
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
            var response = new Response(requestReader.ResourcePath);
            var builder = new ResponseBuilder(response, _services.Modifiers);

            var reader = new RequestReader(requestReader, _services.NameSwitcher, _services.QueryCreator);
            var writer = new ResponseWriter(responder, response, _services.NameSwitcher);
            
            try
            {
                var navigator = new EndpointNavigator(context, _startResourceFactory, _services);
                IRestEndpoint endpoint =  navigator.GetEndpointFromPath(requestReader.ResourcePath);
                IExecutor executor = _services.ExecutorFactory.GetExecutor(endpoint);
                
                await executor.ExecuteAsync(reader, builder);

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