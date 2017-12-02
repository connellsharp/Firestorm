using Firestorm.Stems.Roots;
using Firestorm.Tests.Unit.Stems.Attributes;
using Xunit;

namespace Firestorm.Tests.Unit.Stems.Roots
{
    public class EndpointRootRequestTests
    {
        [Fact]
        public void Ctor_TestContext_UserIsSame()
        {
            var context = new TestEndpointContext();
            context.User = new TestRestUser();

            var request = new EndpointRootRequest(context);

            Assert.Equal(request.User, context.User);
        }

        [Fact]
        public void OnDispose_DisposeOfContext_CallsEvent()
        {
            var context = new TestEndpointContext();
            context.User = new TestRestUser();

            var request = new EndpointRootRequest(context);
            bool disposeEventCalled = false;

            request.OnDispose += delegate { disposeEventCalled = true; };
            context.Dispose();

            Assert.True(disposeEventCalled);
        }
    }
}