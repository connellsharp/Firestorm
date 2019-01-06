using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class FeedbackResponseHeadersModifier : IResponseModifier
    {
        public void AddResource(Response response, ResourceBody resourceBody)
        {
        }

        public void AddOptions(Response response, Options options)
        {
        }

        public void AddAcknowledgment(Response response, Acknowledgment acknowledgment)
        {
            if (acknowledgment is CreatedItemAcknowledgment createdItemAcknowledgment)
            {
                response.StatusCode = HttpStatusCode.Created;
                SetLocationHeader(response, createdItemAcknowledgment.NewIdentifier);
            }
            else
            {
                response.StatusCode = HttpStatusCode.OK;
            }
        }

        public void AddError(Response response, ErrorInfo error)
        {
            response.StatusCode = (HttpStatusCode) error.ErrorStatus;
        }

        public void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems)
        {
            response.StatusCode = (HttpStatusCode) 207;
        }

        private static void SetLocationHeader(Response response, object newIdentifier)
        {
            Debug.Assert(newIdentifier != null, "New identifier cannot be null.");

            var urlCalculator = new UrlCalculator(response.ResourcePath);
            string newUrl = urlCalculator.GetCreatedUrl(newIdentifier);
            response.Headers["Location"] = newUrl;
        }
    }
}