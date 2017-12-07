using System.Collections.Generic;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class AggregateResponseBuilder : IResponseBuilder
    {
        private readonly IEnumerable<IResponseBuilder> _builders;

        public AggregateResponseBuilder(params IResponseBuilder[] builders)
            : this((IEnumerable<IResponseBuilder>) builders)
        { }

        public AggregateResponseBuilder(IEnumerable<IResponseBuilder> builders)
        {
            _builders = builders;
        }

        public void AddResource(Response response, ResourceBody resourceBody)
        {
            foreach (IResponseBuilder builder in _builders)
            {
                builder.AddResource(response, resourceBody);
            }
        }

        public void AddOptions(Response response, Options options)
        {
            foreach (IResponseBuilder builder in _builders)
            {
                builder.AddOptions(response, options);
            }
        }

        public void AddAcknowledgment(Response response, Acknowledgment acknowledgment)
        {
            foreach (IResponseBuilder builder in _builders)
            {
                builder.AddAcknowledgment(response, acknowledgment);
            }
        }

        public void AddError(Response response, ErrorInfo error)
        {
            foreach (IResponseBuilder builder in _builders)
            {
                builder.AddError(response, error);
            }
        }

        public void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems)
        {
            foreach (IResponseBuilder builder in _builders)
            {
                builder.AddMultiFeedback(response, feedbackItems);
            }
        }
    }
}