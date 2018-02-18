using System;
using System.Collections.Generic;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class AggregateResponseModifier : IResponseModifier
    {
        private readonly IEnumerable<IResponseModifier> _modifiers;

        public AggregateResponseModifier(params IResponseModifier[] modifiers)
            : this((IEnumerable<IResponseModifier>) modifiers)
        { }

        public AggregateResponseModifier(IEnumerable<IResponseModifier> modifiers)
        {
            _modifiers = modifiers;
        }

        public void AddResource(Response response, ResourceBody resourceBody)
        {
            foreach (IResponseModifier modifier in _modifiers)
            {
                modifier.AddResource(response, resourceBody);
            }
        }

        public void AddOptions(Response response, Options options)
        {
            foreach (IResponseModifier modifier in _modifiers)
            {
                modifier.AddOptions(response, options);
            }
        }

        public void AddAcknowledgment(Response response, Acknowledgment acknowledgment)
        {
            foreach (IResponseModifier modifier in _modifiers)
            {
                modifier.AddAcknowledgment(response, acknowledgment);
            }
        }

        public void AddError(Response response, ErrorInfo error)
        {
            foreach (IResponseModifier modifier in _modifiers)
            {
                modifier.AddError(response, error);
            }
        }

        public void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems)
        {
            foreach (IResponseModifier modifier in _modifiers)
            {
                modifier.AddMultiFeedback(response, feedbackItems);
            }
        }
    }
}