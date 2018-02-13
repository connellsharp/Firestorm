using System;
using Firestorm.Data;
using Firestorm.Engine.Defaults;
using Firestorm.Stems.Roots.Derive;
using Firestorm.Stems.Roots;
using Firestorm.Tests.Unit;
using Firestorm.Tests.Unit.Endpoints.Stubs;
using Firestorm.Tests.Unit.Engine.Models;

namespace Firestorm.Tests.Functionality.Stems.Models
{
    internal static class StemTestUtility
    {
        public static IRestDirectory GetDirectoryFromRoots(params Type[] rootTypes)
        {
            var stemStartResources = new StemsStartResourceFactory
            {
                RootResourceFactory = new DerivedRootsResourceFactory {
                    RootTypeGetter = new ManualTypeGetter(rootTypes)
                },
                StemConfiguration = new DefaultStemConfiguration()
            };

            stemStartResources.Initialize();

            var directory = (IRestDirectory) stemStartResources.GetStartResource(new TestEndpointContext());
            return directory;
        }

        public static IRestCollection GetArtistsCollection<TStem>()
        {
            IRestDirectory directory = GetDirectoryFromRoots(typeof(ArtistsRoot<TStem>));
            var restCollection = (IRestCollection)directory.GetChild("Artists");
            return restCollection;
        }

        public class ArtistsRoot<TStem> : EngineRoot<Artist>
        {
            public override Type StartStemType { get; } = typeof(TStem);

            protected override IDataTransaction DataTransaction { get; } = new VoidTransaction();

            protected override IEngineRepository<Artist> Repository { get; } = new ArtistMemoryRepository();
        }
    }
}