using System;
using System.Linq;
using Firestorm.Testing.Data;

namespace Firestorm.EntityFramework6.IntegrationTests
{
    public class ExampleFixture : IDisposable
    {
        public ExampleDataContext Context { get; }

        public ExampleFixture()
        {
            Context = new ExampleDataContext();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}