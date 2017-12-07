using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Start
{
    public static class ResponseUtility
    {
        internal static Task WriteResponseAsync(IHttpRequestHandler requestHandler, Response response)
        {
            requestHandler.SetStatusCode(response.StatusCode);

            foreach (var header in response.Headers)
            {
                requestHandler.SetResponseHeader(header.Key, header.Value);
            }

            return requestHandler.SetResponseBodyAsync(response.GetFullBody());
        }

        public static object GetFullBody(this Response response)
        {
            if (response.ExtraBody.Count == 0)
                return response.Body;

            if (response.Body != null)
                response.ExtraBody.Add("resource", response.Body);

            return response.ExtraBody;
        }

        public static void AddFeedback(this IResponseBuilder responseBuilder, Response response, Feedback feedback)
        {
            switch (feedback)
            {
                case AcknowledgmentFeedback acknowledgmentFeedback:
                    responseBuilder.AddAcknowledgment(response, acknowledgmentFeedback.Acknowledgment);
                    break;

                case ErrorFeedback errorFeedback:
                    responseBuilder.AddError(response, errorFeedback.Error);
                    break;

                case MultiFeedback multiFeedback:
                    responseBuilder.AddMultiFeedback(response, multiFeedback.FeedbackItems);
                    break;
            }
        }
    }
}