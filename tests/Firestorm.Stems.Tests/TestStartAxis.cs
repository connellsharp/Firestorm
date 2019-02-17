using System;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.AutoMap;

namespace Firestorm.Stems.Tests
{
    internal class TestStartAxis : IAxis
    {
        public IRestUser User { get; }

        public IStemsCoreServices Services { get; } = new TestStemsServices();

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }

        private class TestStemsServices : IStemsCoreServices
        {
            public IDependencyResolver DependencyResolver { get; }

            public IPropertyAutoMapper AutoPropertyMapper { get; } = new DefaultPropertyAutoMapper();

            public IServiceGroup ServiceGroup { get; } = new ImplementationCache();
        }
    }
}