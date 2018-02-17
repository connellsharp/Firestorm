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
            var builder = new FeedbackResponseHeadersModifier();
            builder.AddMultiFeedback(response, new List<Feedback>());

            Assert.Equal((HttpStatusCode) 207, response.StatusCode);
        }

        [Fact]
        public void AggregateResponseBuilder_EmptyMultiFeedback_Status207()
        {
            var response = new Response(null);
            var builder = new AggregateResponseModifier(new FeedbackResponseHeadersModifier());
            builder.AddMultiFeedback(response, new List<Feedback>());

            Assert.Equal((HttpStatusCode)207, response.StatusCode);
        }

        [Fact]
        public void StatusCodeResponseBuilder_EmptyMultiFeedback_EnumerableBody()
        {
            var response = new Response(null);
            var builder = new StatusCodeResponseModifier();

            builder.AddMultiFeedback(response, new List<Feedback>());
            
            Assert.IsAssignableFrom<IEnumerable>(response.ResourceBody);
        }

        [Fact]
        public void SuccessBooleanResponseBuilder_EmptyMultiFeedback_EnumerableBody()
        {
            var response = new Response(null);
            var builder = new SuccessBooleanResponseModifier();

            builder.AddMultiFeedback(response, new List<Feedback>());
            
            Assert.IsAssignableFrom<IEnumerable>(response.ResourceBody);
        }

        [Fact]
        public void StatusCodeResponseBuilder_MultiFeedback_StatusOk()
        {
            var response = new Response(null);
            var builder = new StatusCodeResponseModifier();

            builder.AddMultiFeedback(response, new List<Feedback>
            {
                new AcknowledgmentFeedback(new Acknowledgment())
            });

            var enumerable = Assert.IsAssignableFrom<IEnumerable>(response.ResourceBody);
            var itemData = enumerable.Cast<RestItemData>().Single();
            Assert.Equal("ok", itemData[StatusCodeResponseModifier.StatusKey]);
        }

        [Fact]
        public void SuccessBooleanResponseBuilder_MultiFeedback_SuccessTrue()
        {
            var response = new Response(null);
            var builder = new SuccessBooleanResponseModifier();

            builder.AddMultiFeedback(response, new List<Feedback>
            {
                new AcknowledgmentFeedback(new Acknowledgment())
            });

            var enumerable = Assert.IsAssignableFrom<IEnumerable>(response.ResourceBody);
            var itemData = enumerable.Cast<RestItemData>().Single();
            Assert.Equal(true, itemData[SuccessBooleanResponseModifier.SuccessKey]);
        }

        [Fact]
        public void StatusCodeResponseBuilder_SingleErrorFeedback_IsCorrectStatus()
        {
            var response = new Response(null);
            var builder = new StatusCodeResponseModifier();

            builder.AddMultiFeedback(response, new List<Feedback>
            {
                new ErrorFeedback(new ErrorInfo(ErrorStatus.InternalServerError, "db", "There was a database error for this one.")),
            });

            var enumerable = Assert.IsAssignableFrom<IEnumerable>(response.ResourceBody);
            var itemData = enumerable.Cast<RestItemData>().Single();
            Assert.Equal("internal_server_error", itemData[StatusCodeResponseModifier.StatusKey]);
        }

        [Fact]
        public void SuccessBooleanResponseBuilder_SingleErrorFeedback_SuccessFalse()
        {
            var response = new Response(null);
            var builder = new SuccessBooleanResponseModifier();

            builder.AddMultiFeedback(response, new List<Feedback>
            {
                new ErrorFeedback(new ErrorInfo(ErrorStatus.InternalServerError, "db", "There was a database error for this one.")),
            });

            var enumerable = Assert.IsAssignableFrom<IEnumerable>(response.ResourceBody);
            var itemData = enumerable.Cast<RestItemData>().Single();
            Assert.Equal(false, itemData[SuccessBooleanResponseModifier.SuccessKey]);
        }

        [Fact]
        public void StatusCodeResponseBuilder_MultiFeedback_CorrectCount()
        {
            var response = new Response(null);
            var builder = new StatusCodeResponseModifier();

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