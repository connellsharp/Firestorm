using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Start.Responses
{
    internal class FeedbackResponseBuilder : IResponseBuilder
    {
        public Task AddResourceAsync(IHttpRequestHandler requestHandler, ResourceBody resourceBody)
        {
            return Task.FromResult(false);
        }

        public Task AddFeedbackAsync(IHttpRequestHandler requestHandler, Feedback feedback)
        {
            HttpStatusCode status = GetStatusCode(feedback);
            requestHandler.SetStatusCode(status);

            if (status == HttpStatusCode.Created)
                SetLocationHeader(requestHandler, feedback);

            return Task.FromResult(false);
        }

        private static void SetLocationHeader(IHttpRequestHandler requestHandler, Feedback feedback)
        {
            object newIdentifier = GetNewIdentifier(feedback);
            Debug.Assert(newIdentifier != null, "Status code 201 should mean there is a new identifier.");
            var urlCalculator = new UrlCalculator(requestHandler);
            string newUrl = urlCalculator.GetCreatedUrl(newIdentifier);
            requestHandler.SetResponseHeader("Location", newUrl);
        }

        private HttpStatusCode GetStatusCode(Feedback feedback)
        {
            if (feedback == null)
                return HttpStatusCode.NoContent;

            switch (feedback.Type)
            {
                case FeedbackType.Acknowledgment:
                    Acknowledgment acknowledgment = ((AcknowledgmentFeedback)feedback).Acknowledgment;
                    return acknowledgment is CreatedItemAcknowledgment ? HttpStatusCode.Created : HttpStatusCode.OK;

                case FeedbackType.Error:
                    ErrorInfo error = ((ErrorFeedback)feedback).Error;
                    return (HttpStatusCode)error.ErrorStatus;

                case FeedbackType.MultiResponse:
                    return (HttpStatusCode)207;

                default:
                    throw new ArgumentOutOfRangeException(nameof(feedback.Type), "Invalid feedback type.");
            }
        }

        public static object GetNewIdentifier(Feedback feedback)
        {
            return ((feedback as AcknowledgmentFeedback)?.Acknowledgment as CreatedItemAcknowledgment)?.NewIdentifier;
        }

        public Task AddOptionsAsync(IHttpRequestHandler requestHandler, Options options)
        {
            return Task.FromResult(false);
        }
    }
}