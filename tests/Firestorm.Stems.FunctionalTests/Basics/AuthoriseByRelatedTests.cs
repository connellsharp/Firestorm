using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Attributes;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Tests.Examples.Music.Data.Models;
using Firestorm.Tests.Examples.Music.Web;
using Firestorm.Testing.Http;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.Tests.Examples.Music.Basics
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
            [AutoExpr]
            public static int TrackID { get; set; }
            
            [Get]
            [AutoExpr]
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

            ResponseAssert.Success(response);
        }

        [Fact]
        public async Task Tracks_GetIDsWhereLiked_AllOdd()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/tracks?fields=id&liked=true&where=id<=10");

            ResponseAssert.Success(response);

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
            HttpResponseMessage response = await HttpClient.GetAsync("/tracks?fields=id&liked=false&where=id<=10");

            ResponseAssert.Success(response);

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