using System.Collections.Generic;
using Firestorm.Endpoints.Responses.Pagination;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    internal class PaginationHeadersResponseModifier : IResponseModifier
    {
        public void AddResource(Response response, ResourceBody resourceBody)
        {
            if (resourceBody is IPagedResourceBody pagedResourceBody)
            {
                var urlCalculator = new UrlCalculator(response.ResourcePath);
                var setter = new LinkHeaderBuilder(urlCalculator);
                setter.AddDetails(pagedResourceBody.PageLinks);
                setter.SetHeaders(response);
            }
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

        public void AddOptions(Response response, Options options)
        {
            // No pagination supported
        }
    }
}