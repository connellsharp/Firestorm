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
            throw new RestApiException((ErrorStatus)response.StatusCode, errorObj.Error + ": " + errorObj.ErrorDescription);
        }

        internal class ErrorModel
        {
            [JsonProperty("error")]
            public string Error { get; set; }

            [JsonProperty("error_description")]
            public string ErrorDescription { get; set; }
        }
    }
}