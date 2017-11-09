using Firestorm.Endpoints;
using Firestorm.Stems.Roots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firestorm.Tests.Stems.Attributes;
using Xunit;

namespace Firestorm.Tests.Stems.Roots
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

    public class TestEndpointContext : IRestEndpointContext
    {
        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }

        public RestEndpointConfiguration Configuration { get; }

        public IRestUser User { get; set; }

        public IRestCollectionQuery GetQuery()
        {
            throw new NotImplementedException();
        }

        public event EventHandler OnDispose;
    }
}
