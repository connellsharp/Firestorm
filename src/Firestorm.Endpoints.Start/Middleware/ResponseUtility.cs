using Firestorm.Core.Web;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Start
{
    public static class ResponseUtility
    {
        public static object GetFullBody(this Response response)
        {
            if (response.ExtraBody.Count == 0)
                return response.ResourceBody;

            if (response.ResourceBody != null)
                response.ExtraBody.Add("resource", response.ResourceBody);

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