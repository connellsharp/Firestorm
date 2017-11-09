using Firestorm.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firestorm.Tests.Stems.Roots
{
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
