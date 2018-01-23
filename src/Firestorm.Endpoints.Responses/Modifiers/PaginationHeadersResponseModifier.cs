using System.Collections.Generic;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

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
        }

        public void AddError(Response response, ErrorInfo error)
        {
        }

        public void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems)
        {
        }

        public void AddOptions(Response response, Options options)
        {
        }
    }
}