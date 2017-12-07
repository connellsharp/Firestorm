using System.Collections.Generic;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    internal class ErrorResponseBuilder : IResponseBuilder
    {
        public void AddResource(Response response, ResourceBody resourceBody)
        {
        }

        public void AddAcknowledgment(Response response, Acknowledgment acknowledgment)
        {
        }

        public void AddError(Response response, ErrorInfo error)
        {
            response.ExtraBody.Add("error", error.ErrorType);
            response.ExtraBody.Add("error_description", error.ErrorDescription);
        }

        public void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems)
        {
        }

        public void AddOptions(Response response, Options options)
        {
        }
    }
}