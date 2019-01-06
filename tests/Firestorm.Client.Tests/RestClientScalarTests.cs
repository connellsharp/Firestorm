using System.Threading.Tasks;
using Firestorm.Client;
using Xunit;

namespace Firestorm.Client.Tests
{
    public class RestClientScalarTests
    {
        private const string BASE_PATH = "scalar/here/siiiick/";
        private const string FULL_BASE_URL = MockHttpClientCreator.BaseUrl + BASE_PATH;

        private readonly MockHttpClientCreator _clientCreator;
        private readonly RestClientScalar _scalar;
        
        public RestClientScalarTests()
        {
            _clientCreator = new MockHttpClientCreator();
            _scalar = new RestClientScalar(_clientCreator, BASE_PATH);
        }

        [Fact]
        public async Task GetScalar_HasSameUri()
        {
            await _scalar.GetAsync();

            Assert.Equal(_clientCreator.LastRequest.RequestUri.ToString(), FULL_BASE_URL);
        }

        [Fact]
        public async Task GetScalar_HasSameValue()
        {
            string str = "this is a string";

            _clientCreator.ResponseBody = @"""" + str + @"""";
            object value = await _scalar.GetAsync();
            
            Assert.Equal(str, value);
        }

        [Fact]
        public async Task EditScalar_HasSameValue()
        {
            string str = "new string";
            await _scalar.EditAsync(str);

            Assert.Equal(@"""" + str + @"""", _clientCreator.LastRequestBody);
        }
    }
}
