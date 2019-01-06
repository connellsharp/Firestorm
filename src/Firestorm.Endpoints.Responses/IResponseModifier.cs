using System.Collections.Generic;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    /// <summary>
    /// Modifies a <see cref="Response"/> object for sending to the client.
    /// </summary>
    public interface IResponseModifier
    {
        void AddResource(Response response, ResourceBody resourceBody);

        void AddOptions(Response response, Options options);

        void AddAcknowledgment(Response response, Acknowledgment acknowledgment);

        void AddError(Response response, ErrorInfo error);

        void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems);
    }
}