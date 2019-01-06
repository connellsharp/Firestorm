using System;
using Firestorm.Data;
using Firestorm.Engine.Defaults;
using Firestorm.Stems;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.Derive;
using Firestorm.Tests.Unit;
using Firestorm.Endpoints.Tests.Stubs;
using Firestorm.Engine.Tests.Models;

namespace Firestorm.Tests.Functionality.Stems.Helpers
{
    internal class StemTestContext
    {
        private IRestDirectory GetDirectoryFromRoots(params Type[] rootTypes)
        {
            TestDependencyResolver.Add(TestRepository);
            
            var stemStartResources = new StemsStartResourceFactory
            {
                RootResourceFactory = new DerivedRootsResourceFactory {
                    RootTypeGetter = new ManualTypeGetter(rootTypes)
                },
                StemConfiguration = new DefaultStemConfiguration
                {
                    DependencyResolver = TestDependencyResolver
                }
            };

            stemStartResources.Initialize();

            var directory = (IRestDirectory) stemStartResources.GetStartResource(new TestRequestContext());
            return directory;
        }

        public TestDependencyResolver TestDependencyResolver { get; } = new TestDependencyResolver();
        
        public ArtistMemoryRepository TestRepository { get; } = new ArtistMemoryRepository();

        public IRestCollection GetArtistsCollection<TStem>()
        {
            IRestDirectory directory = GetDirectoryFromRoots(typeof(ArtistsRoot<TStem>));
            var restCollection = (IRestCollection)directory.GetChild("Artists");
            return restCollection;
        }

        public class ArtistsRoot<TStem> : EngineRoot<Artist>
        {
            public ArtistsRoot(ArtistMemoryRepository repository)
            {
                Repository = repository;
            }
            
            public override Type StartStemType { get; } = typeof(TStem);

            protected override IDataTransaction DataTransaction { get; } = new VoidTransaction();

            protected override IEngineRepository<Artist> Repository { get; }
        }
    }
}