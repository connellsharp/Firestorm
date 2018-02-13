using System;
using Firestorm.Data;
using Firestorm.Stems;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.Derive;
using Firestorm.Tests.Unit;
using Firestorm.Tests.Unit.Endpoints.Stubs;
using Xunit;

namespace Firestorm.Tests.Functionality.Stems
{
    public class StemBehaviorTests
    {
        [Fact]
        public void DisposeEndpointContextDisposesStem()
        {
            var endpointContext = new TestEndpointContext();
            var stemStartResources = new StemsStartResourceFactory
            {
                RootResourceFactory = new DerivedRootsResourceFactory
                {
                    RootTypeGetter = new ManualTypeGetter(typeof(DisposableRoot))
                }
            };
            
            var directory = (IRestDirectory)stemStartResources.GetStartResource(endpointContext);
            var collection = directory.GetChild("Disposable");

            DisposableRoot.DisposeCalled = false;
            endpointContext.Dispose();
            Assert.True(DisposableRoot.DisposeCalled);
        }

        private class DisposableRoot : EngineRoot<Artist>
        {
            public static bool DisposeCalled { get; set; }

            public override void Dispose()
            {
                DisposeCalled = true;
            }

            protected override IDataTransaction DataTransaction { get; } = null;

            protected override IEngineRepository<Artist> Repository { get; } = null;

            public override Type StartStemType { get; } = typeof(TestStem);
        }

        private class TestStem : Stem<Artist>
        { }
    }
}