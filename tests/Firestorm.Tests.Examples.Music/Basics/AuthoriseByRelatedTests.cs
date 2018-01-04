using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Tests.Examples.Data.Models;
using Firestorm.Tests.Examples.Web;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.Tests.Examples.Basics
{
    public class AuthoriseByRelatedTests : IClassFixture<ExampleFixture<AuthoriseByRelatedTests>>
    {
        private HttpClient HttpClient { get; }

        public AuthoriseByRelatedTests(ExampleFixture<AuthoriseByRelatedTests> fixture)
        {
            HttpClient = fixture.HttpClient;
        }

        [DataSourceRoot]
        public class TracksStem : Stem<Track>
        {
            [Identifier]
            [Get(Name = "ID")]
            public static int TrackID { get; set; }
            
            [Get]
            public static string Title { get; set; }

            [Get]
            public Expression<Func<Track, bool>> GetLiked()
            {
                return t => t.LikedByUsers.Any(lu => lu.User.Username == User.Username);
            }
        }

        [Fact]
        public async Task ArtistsCollection_Get_StatusOK()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/tracks");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Tracks_GetIDsWhereLiked_AllOdd()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/tracks?fields=id&liked=true");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string json = await response.Content.ReadAsStringAsync();
            var objs = JsonConvert.DeserializeObject<List<dynamic>>(json);

            Assert.True(objs.Any());

            foreach(dynamic obj in objs)
            {
                int id = obj.id;
                Assert.Equal(1, id % 2); // weird bodge, in db all my likes are odd numbers
            }
        }

        [Fact]
        public async Task Tracks_GetIDsWhereNotLiked_AllEven()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/tracks?fields=id&liked=false");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string json = await response.Content.ReadAsStringAsync();
            var objs = JsonConvert.DeserializeObject<List<dynamic>>(json);

            Assert.True(objs.Any());

            foreach (dynamic obj in objs)
            {
                int id = obj.id;
                Assert.Equal(0, id % 2); // weird bodge, in db all my likes are odd numbers
            }
        }
    }
}