using System;
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

            builder.AppendLine((int)status + " " + status);

            builder.AppendFormat("Error: {0}\r\n", errorObj.Error);
            builder.AppendFormat("Message: {0}\r\n", errorObj.ErrorDescription);

            if (errorObj.InnerDescriptions != null)
            {
                builder.AppendLine();

                foreach (var message in errorObj.InnerDescriptions)
                {
                    builder.AppendFormat("Inner Message: {0}\r\n", message);
                }
            }

            if (errorObj.StackTrace != null)
            {
                builder.AppendLine();
                
                foreach (string line in errorObj.StackTrace)
                    builder.AppendLine(line);

                builder.AppendLine();
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

            [JsonProperty("inner_descriptions")]
            public string[] InnerDescriptions { get; set; }

            [JsonProperty("stack_trace")]
            public string[] StackTrace { get; set; }
        }
    }
}