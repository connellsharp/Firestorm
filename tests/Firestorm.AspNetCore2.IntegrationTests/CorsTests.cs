using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Testing.Http;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.AspNetCore2.IntegrationTests
{
    [UsedImplicitly]
    public class CorsTests: HttpIntegrationTestsBase
    {
        public CorsTests()
        : base(new KestrelIntegrationSuite<CorsStartup>(2225))
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
}