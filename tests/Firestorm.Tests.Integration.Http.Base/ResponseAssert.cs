using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Firestorm.Tests.Integration.Http.Base
{
    public static class ResponseAssert
    {
        public static void Success(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            string errorJson = response.Content.ReadAsStringAsync().Result;

            var errorObj = JsonConvert.DeserializeObject<ErrorModel>(errorJson);

            var status = (ErrorStatus) response.StatusCode;

            throw new RestApiException(status, GetMessage(status, errorObj));
        }

        private static string GetMessage(ErrorStatus status, ErrorModel errorObj)
        {
            var builder = new StringBuilder();

            builder.AppendLine((int) status + " " + status);

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