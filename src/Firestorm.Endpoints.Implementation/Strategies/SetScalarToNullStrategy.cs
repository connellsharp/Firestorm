using System.Threading.Tasks;
using Firestorm.Endpoints.Requests;
using Firestorm.Rest.Web;

namespace Firestorm.Endpoints.Strategies
{
    /// <summary>
    /// Sets a scalar value to null. Good strategy to use for a DELETE request.
    /// </summary>
    public class SetScalarToNullStrategy : ICommandStrategy<IRestScalar>
    {
        public async Task<Feedback> ExecuteAsync(IRestScalar scalar, IEndpointContext context, ResourceBody body)
        {
            Acknowledgment acknowledgment = await scalar.EditAsync(null);
            return new AcknowledgmentFeedback(acknowledgment);
        }
    }
}