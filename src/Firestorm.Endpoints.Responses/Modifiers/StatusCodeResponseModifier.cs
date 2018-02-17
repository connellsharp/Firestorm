using System.Collections.Generic;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class StatusCodeResponseModifier : IResponseModifier
    {
        internal const string StatusKey = "Status";

        public bool WrapResourceObject { get; set; }

        public void AddResource(Response response, ResourceBody resourceBody)
        {
            if (WrapResourceObject)
                response.ExtraBody[StatusKey] = "ok";
        }

        public void AddAcknowledgment(Response response, Acknowledgment acknowledgment)
        {
            if(acknowledgment is CreatedItemAcknowledgment createdItemAcknowledgment)
            {
                response.ExtraBody[StatusKey] = "created";
                response.ExtraBody["Identifier"] = createdItemAcknowledgment.NewIdentifier;
            }
            else
            {
                response.ExtraBody[StatusKey] = "ok";
            }
        }

        public void AddError(Response response, ErrorInfo error)
        {
            string status = error.ErrorStatus.ToString().SeparateCamelCase("_", true);
            response.ExtraBody.Add(StatusKey, status);
        }

        public void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems)
        {
            var looper = new MultiFeedbackLooper(this);
            response.ResourceBody = looper.GetBodyFromMultiFeedback(feedbackItems);
        }

        public void AddOptions(Response response, Options options)
        {
        }
    }
}