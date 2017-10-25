using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Firestorm.Core;
using Firestorm.Core.Web;

namespace Firestorm.Endpoints.Responses
{
    public class FeedbackToResponseConverter
    {
        private readonly IResponseContentGenerator _responseContentGenerator;
        private readonly Feedback _feedback;

        public FeedbackToResponseConverter(IResponseContentGenerator responseContentGenerator, Feedback feedback)
        {
            _responseContentGenerator = responseContentGenerator;
            _feedback = feedback;
        }

        public object GetBody()
        {
            return GetBodyFromFeedback(_feedback);
        }

        private object GetBodyFromFeedback(Feedback feedback)
        {
            if (feedback == null)
                return null;

            switch (feedback.Type)
            {
                case FeedbackType.Acknowledgment:
                    Acknowledgment acknowledgment = ((AcknowledgmentFeedback)feedback).Acknowledgment;
                    return _responseContentGenerator.GetFromAcknowledgment(acknowledgment);

                case FeedbackType.Error:
                    ErrorInfo error = ((ErrorFeedback)feedback).Error;
                    return _responseContentGenerator.GetFromError(error);

                case FeedbackType.MultiResponse:
                    IEnumerable<Feedback> items = ((MultiFeedback)feedback).FeedbackItems;
                    return items.Select(GetBodyFromFeedback).ToArray();

                default:
                    throw new ArgumentOutOfRangeException(nameof(feedback.Type), "Invalid feedback type.");
            }
        }

        public HttpStatusCode GetStatusCode()
        {
            if (_feedback == null)
                return HttpStatusCode.NoContent;

            switch (_feedback.Type)
            {
                case FeedbackType.Acknowledgment:
                    Acknowledgment acknowledgment = ((AcknowledgmentFeedback)_feedback).Acknowledgment;
                    return acknowledgment is CreatedItemAcknowledgment ? HttpStatusCode.Created : HttpStatusCode.OK;

                case FeedbackType.Error:
                    ErrorInfo error = ((ErrorFeedback)_feedback).Error;
                    return (HttpStatusCode)error.ErrorStatus;

                case FeedbackType.MultiResponse:
                    return (HttpStatusCode)207;

                default:
                    throw new ArgumentOutOfRangeException(nameof(_feedback.Type), "Invalid feedback type.");
            }
        }

        public object GetNewIdentifier()
        {
            return ((_feedback as AcknowledgmentFeedback)?.Acknowledgment as CreatedItemAcknowledgment)?.NewIdentifier;
        }
    }
}