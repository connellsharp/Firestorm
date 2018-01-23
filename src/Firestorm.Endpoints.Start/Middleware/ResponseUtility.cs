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

        public static void AddFeedback(this IResponseModifier responseModifier, Response response, Feedback feedback)
        {
            switch (feedback)
            {
                case AcknowledgmentFeedback acknowledgmentFeedback:
                    responseModifier.AddAcknowledgment(response, acknowledgmentFeedback.Acknowledgment);
                    break;

                case ErrorFeedback errorFeedback:
                    responseModifier.AddError(response, errorFeedback.Error);
                    break;

                case MultiFeedback multiFeedback:
                    responseModifier.AddMultiFeedback(response, multiFeedback.FeedbackItems);
                    break;
            }
        }
    }
}