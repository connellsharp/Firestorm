using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Firestorm.Testing.Http
{
    public static class ResponseAssert
    {
        public static void Success(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            ThrowHelpfulAssertionException(response);
        }

        public static void Status(HttpResponseMessage response, HttpStatusCode statusCode)
        {
            if (response.StatusCode == statusCode)
                return;

            ThrowHelpfulAssertionException(response);
        }

        private static void ThrowHelpfulAssertionException(HttpResponseMessage response)
        {
            var status = (ErrorStatus) response.StatusCode;

            string errorJson = response.Content.ReadAsStringAsync().Result;
            var errorObj = JsonConvert.DeserializeObject<ErrorModel>(errorJson);

            string message = GetMessage(status, errorObj);
            throw new RestApiException(status, message);
        }

        private static string GetMessage(ErrorStatus status, ErrorModel errorObj)
        {
            var builder = new StringBuilder();

            builder.AppendLine((int) status + " " + status);

            if(errorObj == null)
            {
                builder.AppendLine("Error response but no response body.");
            }
            else
            {
                builder.AppendFormat("Error: {0}\r\n", errorObj.Error);
                builder.AppendFormat("Description: {0}\r\n", errorObj.ErrorDescription);

                if (errorObj.DeveloperInfo != null)
                {
                    foreach (var info in errorObj.DeveloperInfo.Reverse())
                    {
                        builder.AppendLine();
                        builder.AppendFormat("Message: {0}\r\n", info.Message);

                        if (info.StackTrace != null)
                        {
                            foreach (string line in info.StackTrace)
                                builder.AppendLine(line);

                            builder.AppendLine();
                        }
                    }
                }
                else
                {
                    builder.AppendLine();
                    builder.AppendLine("No developer info was returned in the response.");
                }
            }

            return builder.ToString();
        }

        /// <remarks>
        /// <see cref="ExceptionErrorInfo"/>
        /// </remarks>
        private class ErrorModel
        {
            [JsonProperty("error")]
            public string Error { get; set; }

            [JsonProperty("error_description")]
            public string ErrorDescription { get; set; }

            [JsonProperty("developer_info")]
            public DevInfo[] DeveloperInfo { get; set; }
        }

        private class DevInfo
        {
            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("stack_trace")]
            public string[] StackTrace { get; set; }
        }
    }
}