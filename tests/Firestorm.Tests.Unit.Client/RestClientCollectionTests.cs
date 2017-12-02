using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Client;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.Tests.Unit.Client
{
    public class RestClientCollectionTests
    {
        private const string BASE_PATH = "over/here/yeah/";
        private const string FULL_BASE_URL = MockHttpClientCreator.BaseUrl + BASE_PATH;

        private readonly MockHttpClientCreator _clientCreator;
        private readonly RestClientCollection _collection;
        
        public RestClientCollectionTests()
        {
            _clientCreator = new MockHttpClientCreator();
            _collection = new RestClientCollection(_clientCreator, BASE_PATH);
        }

        [Fact]
        public void GetItem_HasSameHttpClientCreator()
        {
            IRestItem item = _collection.GetItem("test");
            Assert.IsType<RestClientItem>(item);
            Assert.Equal(_clientCreator, ((RestClientItem) item).HttpClientCreator);

            item.GetDataAsync(new[] { "one", "two" });
        }

        [Fact]
        public void GetItem_BuildsCorrectPath()
        {
            IRestItem item = _collection.GetItem("test");
            item.GetDataAsync(new[] { "one", "two" });

            string builtUrl = _clientCreator.LastRequest.RequestUri.ToString();
            Assert.Equal(builtUrl, FULL_BASE_URL + "test?fields=one,two");
        }

        [Fact]
        public void DeleteItem_UsesCorrectPath()
        {
            IRestItem item = _collection.GetItem("deletetest");
            item.DeleteAsync();

            string builtUrl = _clientCreator.LastRequest.RequestUri.ToString();
            Assert.Equal(builtUrl, FULL_BASE_URL + "deletetest");
        }

        [Fact]
        public void DeleteItem_UsesDeleteMethod()
        {
            IRestItem item = _collection.GetItem("deletetest");
            item.DeleteAsync();

            Assert.Equal(HttpMethod.Delete, _clientCreator.LastRequest.Method);
        }

        [Fact]
        public void EditItem_UsesCorrectPath()
        {
            IRestItem item = _collection.GetItem("edittest");
            item.EditAsync(new RestItemData { { "testkey", "testval" } });

            string builtUrl = _clientCreator.LastRequest.RequestUri.ToString();
            Assert.Equal(builtUrl, FULL_BASE_URL + "edittest");
        }

        [Fact]
        public void EditItem_UsesPutMethod()
        {
            IRestItem item = _collection.GetItem("edittest");
            item.EditAsync(new RestItemData { { "testkey", "testval" } });

            Assert.Equal(HttpMethod.Put, _clientCreator.LastRequest.Method);
        }

        [Fact]
        public async Task EditItem_DeserialisesJsonBody()
        {
            IRestItem item = _collection.GetItem("edittest");

            var putObj = new { testkey = "testval" };
            await item.EditAsync(new RestItemData(putObj));
            
            dynamic deserialised = JsonConvert.DeserializeObject(_clientCreator.LastRequestBody, putObj.GetType());

            Assert.Equal(deserialised.testkey, "testval");
        }

        [Fact]
        public async Task AddItem_UsesPostMethod()
        {
            _clientCreator.ResponseStatusCode = HttpStatusCode.Created;
            _clientCreator.ResponseBody = " { success: true, identifier: 321 }";

             await _collection.AddAsync(new RestItemData());

            Assert.Equal(HttpMethod.Post, _clientCreator.LastRequest.Method);
        }

        [Fact]
        public async Task AddItem_FailsIfNotCreated()
        {
            _clientCreator.ResponseStatusCode = HttpStatusCode.OK;

            await Assert.ThrowsAsync<InvalidOperationException>(async delegate
            {
                await _collection.AddAsync(new RestItemData());
            });
        }

        [Fact]
        public async Task AddItem_DeserialisesJsonBody()
        {
            _clientCreator.ResponseStatusCode = HttpStatusCode.Created;
            _clientCreator.ResponseBody = " { success: true, identifier: 321 }";

            var postObj = new { testkey = "testval" };
            await _collection.AddAsync(new RestItemData(postObj));

            dynamic deserialised = JsonConvert.DeserializeObject(_clientCreator.LastRequestBody, postObj.GetType());

            Assert.Equal(postObj.testkey, deserialised.testkey);
        }
    }
}
