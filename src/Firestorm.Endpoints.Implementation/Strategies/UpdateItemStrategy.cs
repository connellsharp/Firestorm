using System.Threading.Tasks;
using Firestorm.Core;
using Firestorm.Core.Web;

namespace Firestorm.Endpoints.Strategies
{
    /// <summary>
    /// Updates only the specified fields on the item and leaves unspecified fields as they are on the server.
    /// This strategy could be used in PATCH or PUT.
    /// </summary>
    internal class UpdateItemStrategy : IUnsafeRequestStrategy<IRestItem>
    {
        public async Task<Feedback> ExecuteAsync(IRestItem item, IRestEndpointContext context, ResourceBody body)
        {
            var itemBody = body as ItemBody;
            if (itemBody == null)
                throw new ItemBodyNotSupportedException(body.ResourceType);

            Acknowledgment acknowledgment = await item.EditAsync(itemBody.Item);
            return new AcknowledgmentFeedback(acknowledgment);
        }
    }
}