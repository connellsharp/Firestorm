using System.Collections.Generic;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    internal class ErrorResponseModifier : IResponseModifier
    {
        public void AddResource(Response response, ResourceBody resourceBody)
        {
            // No error occurred
        }

        public void AddOptions(Response response, Options options)
        {
            // No error occurred
        }

        public void AddAcknowledgment(Response response, Acknowledgment acknowledgment)
        {
            // No error occurred
        }

        public void AddError(Response response, ErrorInfo error)
        {
            response.ExtraBody.Add("Error", error.ErrorType);
            response.ExtraBody.Add("ErrorDescription", error.ErrorDescription);
        }

        public void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems)
        {
            // TODO add error info
        }
    }
}