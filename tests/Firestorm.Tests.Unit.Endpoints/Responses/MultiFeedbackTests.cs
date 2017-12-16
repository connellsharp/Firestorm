using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Responses;
using Xunit;

namespace Firestorm.Tests.Unit.Endpoints.Responses
{
    public class MultiFeedbackTests
    {
        [Fact]
        public void FeedbackResponseHeadersBuilder_EmptyMultiFeedback_Status207()
        {
            var response = new Response(null);
            var builder = new FeedbackResponseHeadersBuilder();
            builder.AddMultiFeedback(response, new List<Feedback>());

            Assert.Equal((HttpStatusCode) 207, response.StatusCode);
        }

        [Fact]
        public void StatusCodeResponseBuilder_EmptyMultiFeedback_EnumerableBody()
        {
            var response = new Response(null);
            var builder = new StatusCodeResponseBuilder();

            builder.AddMultiFeedback(response, new List<Feedback>());
            
            Assert.IsAssignableFrom<IEnumerable>(response.ResourceBody);
        }

        [Fact]
        public void SuccessBooleanResponseBuilder_EmptyMultiFeedback_EnumerableBody()
        {
            var response = new Response(null);
            var builder = new SuccessBooleanResponseBuilder();

            builder.AddMultiFeedback(response, new List<Feedback>());
            
            Assert.IsAssignableFrom<IEnumerable>(response.ResourceBody);
        }

        [Fact]
        public void StatusCodeResponseBuilder_MultiFeedback_StatusOk()
        {
            var response = new Response(null);
            var builder = new StatusCodeResponseBuilder();

            builder.AddMultiFeedback(response, new List<Feedback>
            {
                new AcknowledgmentFeedback(new Acknowledgment())
            });

            var enumerable = Assert.IsAssignableFrom<IEnumerable>(response.ResourceBody);
            var itemData = enumerable.Cast<RestItemData>().Single();
            Assert.Equal("ok", itemData["status"]);
        }

        [Fact]
        public void SuccessBooleanResponseBuilder_MultiFeedback_SuccessTrue()
        {
            var response = new Response(null);
            var builder = new SuccessBooleanResponseBuilder();

            builder.AddMultiFeedback(response, new List<Feedback>
            {
                new AcknowledgmentFeedback(new Acknowledgment())
            });

            var enumerable = Assert.IsAssignableFrom<IEnumerable>(response.ResourceBody);
            var itemData = enumerable.Cast<RestItemData>().Single();
            Assert.Equal(true, itemData["success"]);
        }

        [Fact]
        public void StatusCodeResponseBuilder_SingleErrorFeedback_IsCorrectStatus()
        {
            var response = new Response(null);
            var builder = new StatusCodeResponseBuilder();

            builder.AddMultiFeedback(response, new List<Feedback>
            {
                new ErrorFeedback(new ErrorInfo(ErrorStatus.InternalServerError, "db", "There was a database error for this one.")),
            });

            var enumerable = Assert.IsAssignableFrom<IEnumerable>(response.ResourceBody);
            var itemData = enumerable.Cast<RestItemData>().Single();
            Assert.Equal("internal_server_error", itemData["status"]);
        }

        [Fact]
        public void SuccessBooleanResponseBuilder_SingleErrorFeedback_SuccessFalse()
        {
            var response = new Response(null);
            var builder = new SuccessBooleanResponseBuilder();

            builder.AddMultiFeedback(response, new List<Feedback>
            {
                new ErrorFeedback(new ErrorInfo(ErrorStatus.InternalServerError, "db", "There was a database error for this one.")),
            });

            var enumerable = Assert.IsAssignableFrom<IEnumerable>(response.ResourceBody);
            var itemData = enumerable.Cast<RestItemData>().Single();
            Assert.Equal(false, itemData["success"]);
        }

        [Fact]
        public void StatusCodeResponseBuilder_MultiFeedback_CorrectCount()
        {
            var response = new Response(null);
            var builder = new StatusCodeResponseBuilder();

            builder.AddMultiFeedback(response, new List<Feedback>
            {
                new AcknowledgmentFeedback(new Acknowledgment()),
                new AcknowledgmentFeedback(new Acknowledgment()),
                new ErrorFeedback(new ErrorInfo(ErrorStatus.InternalServerError, "db", "There was a database error for this one.")),
                new AcknowledgmentFeedback(new Acknowledgment()),
            });

            var enumerable = Assert.IsAssignableFrom<IEnumerable>(response.ResourceBody);
            Assert.Equal(4, enumerable.OfType<object>().Count());
        }
    }
}