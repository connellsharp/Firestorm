using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Tests.Examples.Web;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.Tests.Examples.Basics
{
    public class HomeDirectoryTests : IClassFixture<ExampleFixture<HomeDirectoryTests>>
    {
        private HttpClient HttpClient { get; }

        public HomeDirectoryTests(ExampleFixture<HomeDirectoryTests> fixture)
        {
            HttpClient = fixture.HttpClient;
        }

        [Fact]
        public async Task HomeDirectory_Get_StatusOK()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HomeDirectory_Get_Deserialises()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/");
            string json = await response.Content.ReadAsStringAsync();
            JsonConvert.DeserializeObject(json);
        }

        [Fact]
        public async Task HomeDirectory_PostJson_MethodNotAllowed()
        {
            HttpResponseMessage response = await HttpClient.PostAsync("/", new StringContent("{ foo: 987 }"));
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Fact]
        public async Task HomeDirectory_PostCrap_InternalServerError()
        {
            // should this be the case?
            HttpResponseMessage response = await HttpClient.PostAsync("/", new ByteArrayContent(new byte[] { 0, 0, 0, 0, 0 }));
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task HomeDirectory_Put_MethodNotAllowed()
        {
            HttpResponseMessage response = await HttpClient.PutAsync("/", new StringContent("{ foo: 987 }"));
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Fact]
        public async Task HomeDirectory_Delete_MethodNotAllowed()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync("/");
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }
    }
}