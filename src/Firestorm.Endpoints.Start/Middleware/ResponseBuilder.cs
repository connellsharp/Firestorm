using System.Collections.Generic;
using System.Net;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Start
{
    internal class ResponseBuilder : IResponseBuilder
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
            _modifier.AddFeedback(_response, feedback);
        }

        public void SetStatusCode(HttpStatusCode statusCode)
        {
            _response.StatusCode = statusCode;
        }
    }
}