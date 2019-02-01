using System;
using System.Threading.Tasks;
using Firestorm.Endpoints.Responses;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints
{
    public class EndpointsRequestInvoker : IRequestInvoker
    {
        private readonly IStartResourceFactory _startResourceFactory;
        private readonly EndpointApplication _application;

        public EndpointsRequestInvoker(IStartResourceFactory startResourceFactory, IEndpointApplicationFactory applicationFactory)
        {
            _startResourceFactory = startResourceFactory;
            _application = applicationFactory.CreateApplication();
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
            var builder = new ResponseBuilder(response, _application.Modifiers);

            var reader = new RequestReader(requestReader, _application.NameSwitcher, _application.QueryCreator);
            var writer = new ResponseWriter(responder, response, _application.NameSwitcher);
            
            try
            {
                var navigator = new EndpointNavigator(context, _startResourceFactory, _application);
                IRestEndpoint endpoint =  navigator.GetEndpointFromPath(requestReader.ResourcePath);
                IExecutor executor = _application.ExecutorFactory.GetExecutor(endpoint);
                
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