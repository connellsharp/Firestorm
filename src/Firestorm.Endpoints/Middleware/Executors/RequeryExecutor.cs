using System.Diagnostics;
using System.Threading.Tasks;
using Firestorm.Endpoints.Responses;
using Firestorm.Rest.Web;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// An <see cref="EndpointExecutor"/> that returns the resource on POST, PUT and PATCH.
    /// </summary>
    internal class RequeryExecutor : IExecutor
    {
        private readonly IExecutor _executor;

        public RequeryExecutor(IExecutor executor)
        {
            _executor = executor;
        }

        public IRestEndpoint Endpoint
        {
            get { return _executor.Endpoint; }
        }

        public async Task<object> ExecuteAsync(IRequestReader reader, ResponseBuilder response)
        {
            switch (reader.RequestMethod)
            {
                case "POST":
                case "PUT":
                case "PATCH":
                    return await ExecuteWithRequeryAsync(reader, response);

                default:
                    return _executor.ExecuteAsync(reader, response);
            }
        }

        private async Task<object> ExecuteWithRequeryAsync(IRequestReader reader, ResponseBuilder response)
        {
            object returnValue = await _executor.ExecuteAsync(reader, response);

            IRestEndpoint endpoint = _executor.Endpoint;

            if (returnValue is AcknowledgmentFeedback ackFeedback
                && ackFeedback.Acknowledgment is CreatedItemAcknowledgment created)
            {
                endpoint = endpoint.Next(new RawNextPath(created.NewIdentifier.ToString()));
                Debug.Assert(endpoint != null);
            }

            ResourceBody resourceBody = await endpoint.GetAsync(reader.GetQuery());
            response.AddResource(resourceBody);

            return returnValue;
        }
    }
}