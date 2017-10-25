using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Tests.Examples.Data.Models;
using Firestorm.Tests.Examples.Web;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.Tests.Examples.Basics
{
    public class ComplexFieldGettersTests : IClassFixture<ExampleFixture<ComplexFieldGettersTests>>
    {
        private HttpClient HttpClient { get; }

        public ComplexFieldGettersTests(ExampleFixture<ComplexFieldGettersTests> fixture)
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

            [Get(Argument = nameof(Title))]
            public object GetComplex(string title) // TODO not a real world example.. this in integrations tests though.
            {
                return new
                {
                    title = title,
                    nice = "object",
                    look = "at this"
                };
            }
        }

        [Fact]
        public async Task Collection_GetWithComplexField_StatusOK()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/tracks?fields=id,title,complex");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Item_GetWithComplexField_StatusOK()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/tracks/1?fields=id,title,complex");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Scalar_GetComplexField_StatusOK()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/tracks/1/complex");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}