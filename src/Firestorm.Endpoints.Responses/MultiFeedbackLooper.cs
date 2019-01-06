using System;
using System.Collections.Generic;
using System.Linq;
using Firestorm.Rest.Web;

namespace Firestorm.Endpoints.Responses
{
    internal class MultiFeedbackLooper
    {
        private readonly IResponseModifier _modifier;

        public MultiFeedbackLooper(IResponseModifier modifier)
        {
            _modifier = modifier;
        }

        public object GetBodyFromMultiFeedback(IEnumerable<Feedback> feedbackItems)
        {
            return feedbackItems.Select(GetBodyFromFeedback);
        }

        public object GetBodyFromFeedback(Feedback feedback)
        {
            if (feedback == null)
                return null;

            var response = new Response(null);

            switch (feedback)
            {
                case AcknowledgmentFeedback acknowledgmentFeedback:
                    _modifier.AddAcknowledgment(response, acknowledgmentFeedback.Acknowledgment);
                    return response.ExtraBody;

                case ErrorFeedback errorFeedback:
                    _modifier.AddError(response, errorFeedback.Error); // TODO showDeveloperErrors ?
                    return response.ExtraBody;

                case MultiFeedback multiFeedback:
                    return multiFeedback.FeedbackItems.Select(GetBodyFromFeedback).ToArray();

                default:
                    throw new ArgumentOutOfRangeException(nameof(feedback.Type), "Invalid feedback type.");
            }
        }
    }
}