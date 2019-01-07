using System;
using Firestorm.Data;
using Firestorm.Engine.Defaults;
using Firestorm.Stems.Roots.Derive;
using Firestorm.Testing;
using Firestorm.Engine.Tests.Models;

namespace Firestorm.Stems.IntegrationTests.Models
{
    internal class ArtistsRoot : EngineRoot<Artist>
    {
        protected override IDataTransaction DataTransaction { get; } = new VoidTransaction();

        protected override IEngineRepository<Artist> Repository { get; } = new ArtistMemoryRepository();

        public override Type StartStemType { get; } = typeof(ArtistsStem);
    }
}