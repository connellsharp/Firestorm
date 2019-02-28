using System.Collections.Generic;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class PagedBodyResponseModifier : IResponseModifier
    {
        public void AddResource(Response response, ResourceBody resourceBody)
        {
            if (resourceBody is IPagedResourceBody pagedResource)
            {
                response.ExtraBody["Page"] = pagedResource.PageLinks;
            }
        }

        public void AddOptions(Response response, Options options)
        {
            // No pagination supported
        }

        public void AddAcknowledgment(Response response, Acknowledgment acknowledgment)
        {
            // No collection
        }

        public void AddError(Response response, ErrorInfo error)
        {
            // No collection
        }

        public void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems)
        {
            // No pagination supported
        }
    }
}