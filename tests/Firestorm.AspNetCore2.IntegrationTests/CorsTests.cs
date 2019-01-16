using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Testing.Http;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.AspNetCore2.IntegrationTests
{
    [UsedImplicitly]
    public class CorsTests: HttpIntegrationTestsBase, IClassFixture<CorsFixture>
    {
        public CorsTests(CorsFixture fixture)
            : base(fixture.IntegrationSuite)
        {
        }

        [Fact]
        public async Task CorsHeaders_Send_CorsResponse()
        {
            var response = await HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Options, "/")
            {
                Headers =
                {
                    { "Access-Control-Request-Headers", "content-type" },
                    { "Access-Control-Request-Method", "GET" },
                    { "Origin", "https://example.com" }
                }
            });
            
            ResponseAssert.Success(response);
        }
    }

    public class CorsFixture : HttpFixture<OptionsStartup>
    {
        public CorsFixture()
            : base(2225)
        {
        }
    }
}