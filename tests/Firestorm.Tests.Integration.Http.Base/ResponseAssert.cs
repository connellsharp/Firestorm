using System.Net.Http;
using Newtonsoft.Json;

namespace Firestorm.Tests.Integration.Http.Base
{
    internal static class ResponseAssert
    {
        internal static void Success(HttpResponseMessage response)
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
            string message = ((int)status) + " " + status + "\r\nError: " + errorObj.Error + "\r\nMessage: " + errorObj.ErrorDescription;

            if (errorObj.StackTrace != null)
                foreach (string line in errorObj.StackTrace)
                    message += "\r\n" + line;

            if (errorObj.InnerDescriptions != null)
                return message + "\r\n\r\n" + string.Join("\r\n", errorObj.InnerDescriptions);

            return message;

        }

        internal class ErrorModel
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