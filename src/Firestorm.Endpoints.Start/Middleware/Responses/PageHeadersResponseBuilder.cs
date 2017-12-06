using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Start.Responses
{
    internal class PageHeadersResponseBuilder : IResponseBuilder
    {
        public Task AddResourceAsync(IHttpRequestHandler requestHandler, ResourceBody resourceBody)
        {
            if (resourceBody is IPagedResourceBody pagedResourceBody)
            {
                var setter = new LinkHeaderBuilder(new UrlCalculator(requestHandler));
                setter.AddDetails(pagedResourceBody.PageLinks);
                setter.SetHeaders(requestHandler);
            }

            return Task.FromResult(false);
        }

        public Task AddFeedbackAsync(IHttpRequestHandler requestHandler, Feedback feedback)
        {
            return Task.FromResult(false);
        }

        public Task AddOptionsAsync(IHttpRequestHandler requestHandler, Options options)
        {
            return Task.FromResult(false);
        }
    }
}