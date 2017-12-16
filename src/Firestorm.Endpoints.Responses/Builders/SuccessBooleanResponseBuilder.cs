using System.Collections.Generic;
using System.Linq;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class SuccessBooleanResponseBuilder : IResponseBuilder
    {
        public bool WrapResourceObject { get; set; }

        public void AddResource(Response response, ResourceBody resourceBody)
        {
            if (WrapResourceObject)
                response.ExtraBody["success"] = true;
        }

        public void AddAcknowledgment(Response response, Acknowledgment acknowledgment)
        {
            response.ExtraBody["success"] = true;

            if (acknowledgment is CreatedItemAcknowledgment createdItemAcknowledgment)
            {
                response.ExtraBody["identifier"] = createdItemAcknowledgment.NewIdentifier;
            }
        }

        public void AddError(Response response, ErrorInfo error)
        {
            response.ExtraBody["success"] = false;
        }

        public void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems)
        {
            var looper = new MultiFeedbackLooper(this);
            response.ResourceBody = feedbackItems.Select(looper.GetBodyFromFeedback);
        }

        public void AddOptions(Response response, Options options)
        {
        }
    }
}