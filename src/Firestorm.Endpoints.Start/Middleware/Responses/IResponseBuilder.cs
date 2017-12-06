using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Start.Responses
{
    internal interface IResponseBuilder
    {
        Task AddResourceAsync(IHttpRequestHandler requestHandler, ResourceBody resourceBody);

        Task AddFeedbackAsync(IHttpRequestHandler requestHandler, Feedback feedback);

        Task AddOptionsAsync(IHttpRequestHandler requestHandler, Options options);
    }
}