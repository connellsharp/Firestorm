using System.Net;
using System.Threading.Tasks;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Executors
{
    internal class PreconditionsExecutor : IExecutor
    {
        private readonly IExecutor _executor;

        public PreconditionsExecutor(IExecutor executor)
        {
            _executor = executor;
        }

        public IRestEndpoint Endpoint
        {
            get { return _executor.Endpoint; }
        }

        public async Task<object> ExecuteAsync(IRequestReader reader, ResponseBuilder response)
        {
            return await CheckPreconditions(reader, response)
                ? await _executor.ExecuteAsync(reader, response)
                : null;
        }

        private async Task<bool> CheckPreconditions(IRequestReader reader, ResponseBuilder response)
        {
            switch (reader.RequestMethod)
            {
                case "GET":
                    return await CheckGetAsync(reader, response);

                case "POST":
                case "PUT":
                case "PATCH":
                case "DELETE":
                    return await CheckModifyAsync(reader, response);

                default:
                    return true;
            }
        }

        private async Task<bool> CheckGetAsync(IRequestReader reader, ResponseBuilder response)
        {
            if (Endpoint.EvaluatePreconditions(reader.GetPreconditions()))
                return true;
            
            response.SetStatusCode(HttpStatusCode.NotModified);
            return false;
        }

        private async Task<bool> CheckModifyAsync(IRequestReader reader, ResponseBuilder response)
        {
            if (Endpoint.EvaluatePreconditions(reader.GetPreconditions()))
                return true;
            
            response.SetStatusCode(HttpStatusCode.PreconditionFailed);
            return false;
        }
    }
}