using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Attributes;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Tests.Examples.Music.Data.Models;
using Firestorm.Tests.Examples.Music.Web;namespace Firestorm.Tests.Examples.Music.Basics
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
            [AutoExpr]
            public static int TrackID { get; set; }
            
            [Get]
            [AutoExpr]
            public static string Title { get; set; }

            [Get(Argument = nameof(Title))]
            public object GetComplex(string title) // TODO not a real world example.. this in integrations tests though.
            {
                return new
                {
                    Title = title,
                    Nice = "object",
                    Look = "at this"
                };
            }
        }

        [Fact]
        public async Task Collection_GetWithComplexField_StatusOK()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/tracks?fields=id,title,complex");

            ResponseAssert.Success(response);
        }

        [Fact]
        public async Task Item_GetWithComplexField_StatusOK()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/tracks/1?fields=id,title,complex");

            ResponseAssert.Success(response);
        }

        [Fact]
        public async Task Scalar_GetComplexField_StatusOK()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/tracks/1/complex");

            ResponseAssert.Success(response);
        }
    }
}