using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Start.Responses
{
    internal class ResponseBodyBuilder : IResponseBuilder
    {
        private readonly IResponseContentGenerator _responseContentGenerator;
        private readonly bool _showDeveloperErrors;

        public ResponseBodyBuilder(IResponseContentGenerator responseContentGenerator, bool showDeveloperErrors)
        {
            _responseContentGenerator = responseContentGenerator;
            _showDeveloperErrors = showDeveloperErrors;
        }

        public async Task AddResourceAsync(IHttpRequestHandler requestHandler, ResourceBody resourceBody)
        {
            object resBody = _responseContentGenerator.GetFromResource(resourceBody);
            await requestHandler.SetResponseBodyAsync(resBody);
        }

        public Task AddFeedbackAsync(IHttpRequestHandler requestHandler, Feedback feedback)
        {
            GetBodyFromFeedback(feedback);
            return Task.FromResult(false);
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
                    return _responseContentGenerator.GetFromError(error, _showDeveloperErrors);

                case FeedbackType.MultiResponse:
                    IEnumerable<Feedback> items = ((MultiFeedback)feedback).FeedbackItems;
                    return items.Select(GetBodyFromFeedback).ToArray();

                default:
                    throw new ArgumentOutOfRangeException(nameof(feedback.Type), "Invalid feedback type.");
            }
        }

        public async Task AddOptionsAsync(IHttpRequestHandler requestHandler, Options options)
        {
            object optionsBody = _responseContentGenerator.GetFromOptions(options);
            await requestHandler.SetResponseBodyAsync(optionsBody);
        }
    }
}