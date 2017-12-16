using System.Collections.Generic;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Start
{
    internal class ResponseWriter
    {
        private readonly IHttpRequestHandler _requestHandler;
        private readonly IResponseBuilder _builder;
        private readonly Response _response;

        public ResponseWriter(IHttpRequestHandler requestHandler, IResponseBuilder builder)
        {
            _requestHandler = requestHandler;
            _builder = builder;
            _response = new Response(requestHandler.ResourcePath);
        }

        public void AddResource(ResourceBody resourceBody)
        {
            _builder.AddResource(_response, resourceBody);
        }

        public void AddOptions(Options options)
        {
            _builder.AddOptions(_response, options);
        }

        public void AddAcknowledgment(Acknowledgment acknowledgment)
        {
            _builder.AddAcknowledgment(_response, acknowledgment);
        }

        public void AddError(ErrorInfo error)
        {
            _builder.AddError(_response, error);
        }

        public void AddMultiFeedback(IEnumerable<Feedback> feedbackItems)
        {
            _builder.AddMultiFeedback(_response, feedbackItems);
        }

        public void AddFeedback(Feedback feedback)
        {
            _builder.AddFeedback(_response, feedback);
        }

        public Task WriteAsync()
        {
            _requestHandler.SetStatusCode(_response.StatusCode);

            foreach (var header in _response.Headers)
            {
                _requestHandler.SetResponseHeader(header.Key, header.Value);
            }

            return _requestHandler.SetResponseBodyAsync(_response.GetFullBody());
        }
    }
}