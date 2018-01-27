using System.Net.Http;
using Newtonsoft.Json;

namespace Firestorm.Tests.Examples.Football.Tests
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
            string message = ((int)status) + " " + status + " - " + errorObj.Error + " - " + errorObj.ErrorDescription;

            if (errorObj.InnerDescriptions == null)
                return message;

            return message + "\r\n\r\n" + string.Join("\r\n", errorObj.InnerDescriptions);
        }

        internal class ErrorModel
        {
            [JsonProperty("error")]
            public string Error { get; set; }

            [JsonProperty("error_description")]
            public string ErrorDescription { get; set; }

            [JsonProperty("inner_descriptions")]
            public string[] InnerDescriptions { get; set; }
        }
    }
}