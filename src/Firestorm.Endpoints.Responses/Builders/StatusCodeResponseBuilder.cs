using System.Collections.Generic;
using System.Linq;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class StatusCodeResponseBuilder : IResponseBuilder
    {
        public bool WrapResourceObject { get; set; }

        public void AddResource(Response response, ResourceBody resourceBody)
        {
            if (WrapResourceObject)
                response.ExtraBody["status"] = "ok";
        }

        public void AddAcknowledgment(Response response, Acknowledgment acknowledgment)
        {
            if(acknowledgment is CreatedItemAcknowledgment createdItemAcknowledgment)
            {
                response.ExtraBody["status"] = "created";
                response.ExtraBody["identifier"] = createdItemAcknowledgment.NewIdentifier;
            }
            else
            {
                response.ExtraBody["status"] = "ok";
            }
        }

        public void AddError(Response response, ErrorInfo error)
        {
            string status = error.ErrorStatus.ToString().SeparateCamelCase("_", true);
            response.ExtraBody.Add("status", status);
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