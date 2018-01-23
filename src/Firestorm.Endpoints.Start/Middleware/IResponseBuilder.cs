using System.Net;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Start
{
    internal interface IResponseBuilder
    {
        void AddError(ErrorInfo error);

        void AddFeedback(Feedback feedback);

        void AddOptions(Options options);

        void AddResource(ResourceBody resourceBody);

        void SetStatusCode(HttpStatusCode statusCode);
    }
}