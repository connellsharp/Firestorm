using System;
using Firestorm.Data;
using Firestorm.Engine.Defaults;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.Combined;
using Firestorm.Stems.Roots.Derive;
using Firestorm.Testing.Http;
using Firestorm.Testing.Http.Models;
using Firestorm.Testing.Models;

namespace Firestorm.Stems.IntegrationTests.Helpers
{
    /// <summary>
    /// Class for getting test collections and directories for Stems integration tests.
    /// Uses <see cref="ArtistMemoryRepository"/> in Derived Roots.
    /// </summary>
    internal class StemTestContext
    {
        private IRestDirectory GetDirectoryFromRoots(params Type[] rootTypes)
        {
            TestDependencyResolver.Add(TestRepository);

            var testServices = new StemsServices
            {
                DependencyResolver = TestDependencyResolver,
                ServiceGroup = new DefaultServiceGroup(),
                AutoPropertyMapper = new DefaultPropertyAutoMapper()
            };

            var startInfoFactory = new DerivedRootStartInfoFactory(new ManualTypeGetter(rootTypes));
            
            var stemStartResources = new StemsStartResourceFactory(testServices, startInfoFactory);

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