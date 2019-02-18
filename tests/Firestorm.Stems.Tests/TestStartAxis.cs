using System;

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
    }
}