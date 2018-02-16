using System.Collections.Generic;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

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