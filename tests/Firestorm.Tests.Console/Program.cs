using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Client;
using Firestorm.Endpoints.WebApi;
using Firestorm.Tests.Models;
using Microsoft.Owin.Hosting;

namespace Firestorm.Tests.Console
{
    public static class Program
    {
        public static void Main()
        {

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: Startup.BaseAddress))
            {
                TestRestRequestAsync(Startup.BaseAddress).Wait();

                System.Console.WriteLine("Press any key to exit...");
                System.Console.ReadLine();
            }
        }

        private static async Task TestRestRequestAsync(string baseAddress)
        {
            var client = new RestClient(baseAddress + "rest/");

            var artist = await client
                .RequestCollection("artists3")
                .GetItem("123")
                .GetDataAsync(new[] { "name" })
                .As<Artist>();

            System.Console.WriteLine(artist.Name);
        }
    }
}