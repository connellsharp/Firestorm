using System;
using Firestorm.Data;
using Firestorm.Engine.Defaults;
using Firestorm.Stems.Roots.Derive;
using Firestorm.Tests.Endpoints.Models;
using Firestorm.Tests.Engine.Models;
using Firestorm.Tests.Models;
using Firestorm.Stems.Roots;

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
                StemConfiguration = new DefaultStemConfiguration
                {
                    NamingConventionSwitcher = null
                }
            };

            stemStartResources.Initialize();

            var directory = (IRestDirectory) stemStartResources.GetStartResource(new TestEndpointContext());
            return directory;
        }

        public static IRestCollection GetArtistsCollection<TStem>()
        {
            IRestDirectory directory = GetDirectoryFromRoots(typeof(ArtistsRoot<TStem>));
            var restCollection = (IRestCollection)directory.GetChild("artists");
            return restCollection;
        }

        public class ArtistsRoot<TStem> : EngineRoot<Artist>
        {
            public override Type StartStemType { get; } = typeof(TStem);

            protected override IDataTransaction DataTransaction { get; } = new TestTransaction();

            protected override IEngineRepository<Artist> Repository { get; } = new ArtistMemoryRepository();
        }
    }
}