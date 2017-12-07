using System.Collections.Generic;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class DirectResponseBuilder : IResponseBuilder
    {
        public void AddResource(Response response, ResourceBody resourceBody)
        {
            response.Body = resourceBody.GetObject();
        }

        public void AddOptions(Response response, Options options)
        {
            throw new System.NotImplementedException();
        }

        public void AddAcknowledgment(Response response, Acknowledgment acknowledgment)
        {
        }

        public void AddError(Response response, ErrorInfo error)
        {
        }

        public void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems)
        {
        }
    }
}