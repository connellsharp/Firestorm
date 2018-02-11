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
    }
}