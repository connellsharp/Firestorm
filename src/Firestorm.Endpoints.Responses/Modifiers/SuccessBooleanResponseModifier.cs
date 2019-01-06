using System.Collections.Generic;
using System.Linq;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class SuccessBooleanResponseModifier : IResponseModifier
    {
        internal const string SuccessKey = "Success";

        public bool WrapResourceObject { get; set; }

        public void AddResource(Response response, ResourceBody resourceBody)
        {
            if (WrapResourceObject)
                response.ExtraBody[SuccessKey] = true;
        }

        public void AddAcknowledgment(Response response, Acknowledgment acknowledgment)
        {
            response.ExtraBody[SuccessKey] = true;

            if (acknowledgment is CreatedItemAcknowledgment createdItemAcknowledgment)
            {
                response.ExtraBody["Identifier"] = createdItemAcknowledgment.NewIdentifier; // TODO Identifier name?
            }
        }

        public void AddError(Response response, ErrorInfo error)
        {
            response.ExtraBody[SuccessKey] = false;
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