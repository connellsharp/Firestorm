using System.Collections.Generic;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public interface IResponseBuilder
    {
        void AddResource(Response response, ResourceBody resourceBody);

        void AddOptions(Response response, Options options);

        void AddAcknowledgment(Response response, Acknowledgment acknowledgment);

        void AddError(Response response, ErrorInfo error);

        void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems);
    }
}