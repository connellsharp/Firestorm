using Firestorm.Endpoints.Responses;
using Xunit;

namespace Firestorm.Tests.Unit.Endpoints.Responses
{
    public class LinkHeaderBuilderTests
    {
        [Fact]
        public void NextOnly_AddDetails_CorrectHeader()
        {
            var builder = new LinkHeaderBuilder(new UrlCalculator("/films"));

            builder.AddDetails(new Core.Web.PageLinks
            {
                Next = new PageInstruction { Offset = 600, Size = 100 }
            });

            string headerValue = builder.GetHeaderValue();
            Assert.Equal("</films?offset=600&size=100>;rel=\"next\"", headerValue);
        }

        [Fact]
        public void NextAndPrev_AddDetails_CorrectHeader()
        {
            var builder = new LinkHeaderBuilder(new UrlCalculator("/films"));

            builder.AddDetails(new Core.Web.PageLinks
            {
                Next = new PageInstruction { Offset = 600, Size = 100 },
                Previous = new PageInstruction { Offset = 400, Size = 100 },
            });

            string headerValue = builder.GetHeaderValue();
            Assert.Equal("</films?offset=600&size=100>;rel=\"next\", </films?offset=400&size=100>;rel=\"prev\"", headerValue);
        }
    }
}