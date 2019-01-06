using System.Collections.Generic;
using System.Net;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class ResponseBuilder
    {
        private readonly IResponseModifier _modifier;
        private readonly Response _response;

        public ResponseBuilder(Response response, IResponseModifier modifier)
        {
            _response = response;
            _modifier = modifier;
        }

        public ResponseBuilder(Response response, IEnumerable<IResponseModifier> modifiers)
        {
            _response = response;
            _modifier = new AggregateResponseModifier(modifiers);
        }

        public void AddResource(ResourceBody resourceBody)
        {
            _modifier.AddResource(_response, resourceBody);
        }

        public void AddOptions(Options options)
        {
            _modifier.AddOptions(_response, options);
        }

        public void AddError(ErrorInfo error)
        {
            _modifier.AddError(_response, error);
        }

        public void AddFeedback(Feedback feedback)
        {
            switch (feedback)
            {
                case AcknowledgmentFeedback acknowledgmentFeedback:
                    _modifier.AddAcknowledgment(_response, acknowledgmentFeedback.Acknowledgment);
                    break;

                case ErrorFeedback errorFeedback:
                    _modifier.AddError(_response, errorFeedback.Error);
                    break;

                case MultiFeedback multiFeedback:
                    _modifier.AddMultiFeedback(_response, multiFeedback.FeedbackItems);
                    break;
            }
        }

        public void SetStatusCode(HttpStatusCode statusCode)
        {
            _response.StatusCode = statusCode;
        }
    }
}