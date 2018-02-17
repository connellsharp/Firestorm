using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Web
{
    public static class ResponseUtility
    {
        internal const string ResourceKey = "Resource";

        public static object GetFullBody(this Response response)
        {
            if (response.ExtraBody.Count == 0)
                return response.ResourceBody;

            if (response.ResourceBody != null)
                response.ExtraBody.Add(ResourceKey, response.ResourceBody);

            return response.ExtraBody;
        }
    }
}