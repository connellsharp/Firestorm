using System;
using System.Threading.Tasks;
using Firestorm.Rest.Web;

namespace Firestorm.Endpoints.Strategies
{
    internal class ReplaceScalarStrategy : IUnsafeRequestStrategy<IRestScalar>
    {
        public async Task<Feedback> ExecuteAsync(IRestScalar scalar, IEndpointContext context, ResourceBody body)
        {
            var scalarBody = body as ScalarBody;
            if (scalarBody == null)
                throw new InvalidCastException("Request body type must be a scalar value for this endpoint.");

            Acknowledgment acknowledgment = await scalar.EditAsync(scalarBody.Scalar);
            return new AcknowledgmentFeedback(acknowledgment);
        }
    }
}