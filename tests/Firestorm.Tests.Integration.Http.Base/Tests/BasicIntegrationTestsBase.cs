using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Tests.Models;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.Tests.Integration.Http.Base.Tests
{
    public abstract class BasicIntegrationTestsBase : HttpIntegrationTestsBase
    {
        protected BasicIntegrationTestsBase(IHttpIntegrationSuite integrationSuite)
            : base(integrationSuite)
        { }

        [Fact]
        public async Task RootDirectory_Get_StatusOK()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/");
            ResponseAssert.Success(response);
        }

        [Fact]
        public async Task ArtistsCollection_Get_StatusOK()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists");
            ResponseAssert.Success(response);
        }

        [Fact]
        public async Task ArtistItem_Get_Deserialises()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists/123");
            ResponseAssert.Success(response);

            string json = await response.Content.ReadAsStringAsync();
            object obj = JsonConvert.DeserializeObject(json);
        }

        [Fact]
        public async Task ArtistNameScalar_Get_Correct()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists/123/name");
            ResponseAssert.Success(response);

            string responseStr = await response.Content.ReadAsStringAsync();
            Assert.Equal("\"" + TestRepositories.ArtistName + "\"", responseStr);
        }

        [Fact]
        public async Task NonExistentItemID_Get_404()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists/321");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            string json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);

            Assert.NotNull(obj);
            Assert.Equal("item_with_identifier_not_found", (string)obj.error);
        }

        [Fact]
        public async Task NonExistentScalarField_Get_404()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists/123/ohdear");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            string json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);

            Assert.NotNull(obj);
            Assert.Equal("field_not_found", (string)obj.error);
        }

        [Fact]
        public async Task ArtistsCollection_GetWithFields_DeserialisesAndCorrect()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists?fields=id,name");
            ResponseAssert.Success(response);

            string json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);
            Assert.NotNull(obj);

            string name = obj[0].name;
            Assert.Equal(TestRepositories.ArtistName, name);
        }
    }
}
