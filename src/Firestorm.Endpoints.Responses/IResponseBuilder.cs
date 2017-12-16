using System.Collections.Generic;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    /// <summary>
    /// Adds to a <see cref="Response"/> object for sending to the client.
    /// </summary>
    /// <remarks>
    /// Not sure this is really a "builder" if the response is passed in and several builders are combined to build the full response.
    /// Visitor, maybe?
    /// </remarks>
    public interface IResponseBuilder
    {
        void AddResource(Response response, ResourceBody resourceBody);

        void AddOptions(Response response, Options options);

        void AddAcknowledgment(Response response, Acknowledgment acknowledgment);

        void AddError(Response response, ErrorInfo error);

        void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems);
    }
}