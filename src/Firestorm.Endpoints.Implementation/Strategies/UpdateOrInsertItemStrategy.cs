using System;
using System.Threading.Tasks;
using Firestorm.Endpoints.Requests;
using Firestorm.Rest.Web;

namespace Firestorm.Endpoints.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Not sure what's gonna happen here as we use <see cref="IdentifierInfo"/> to decide if upsert is enabled atm.</remarks>
    internal class UpdateOrInsertItemStrategy : IUnsafeRequestStrategy<IRestItem>
    {
        public async Task<Feedback> ExecuteAsync(IRestItem item, IEndpointContext context, ResourceBody body)
        {
            throw new NotImplementedException();

            var itemBody = body as ItemBody;
            if (itemBody == null)
                throw new ItemBodyNotSupportedException(body.ResourceType);

            Acknowledgment acknowledgment = await item.EditAsync(itemBody.Item);
            return new AcknowledgmentFeedback(acknowledgment);
        }
    }
}