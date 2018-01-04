using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Tests.Examples.Data.Models;
using Firestorm.Tests.Examples.Web;
using Xunit;
using Newtonsoft.Json;

namespace Firestorm.Tests.Examples.Basics
{
    public class OptionsTests : IClassFixture<ExampleFixture<OptionsTests>>
    {
        private HttpClient HttpClient { get; }

        public OptionsTests(ExampleFixture<OptionsTests> fixture)
        {
            HttpClient = fixture.HttpClient;
        }

        [DataSourceRoot]
        public class ArtistsStem : Stem<Artist>
        {
            [Identifier(Name = "id")]
            [Get(Display.Nested)]
            [Description("The unique numerical ID of the artist.")]
            public static Expression<Func<Artist, int>> ID
            {
                get { return a => a.ArtistID; }
            }

            [Identifier(Name = "name")]
            [Get]
            [Description("The official name of the artist.")]
            public static Expression<Func<Artist, string>> Name
            {
                get { return a => a.Name; }
            }
        }

        [Fact]
        public async Task Collection_Options_Deserialises()
        {
            HttpResponseMessage response = await HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Options, "/artists"));
            string json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);
        }

        [Fact]
        public async Task Item_Options_Deserialises()
        {
            HttpResponseMessage response = await HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Options, "/artists/1"));
            string json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);
        }
    }
}