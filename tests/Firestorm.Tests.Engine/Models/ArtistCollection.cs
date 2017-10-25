using System;
using Firestorm.Engine;
using Firestorm.Tests.Models;

namespace Firestorm.Tests.Engine.Models
{
    //[RestStartDirectory("artists")]
    [Obsolete("This worked before but replaced with use of DefaultStartResourceFactory.")]
    public class ArtistCollection : EngineRestCollection<Artist>
    {
        public ArtistCollection()
            : base(new CodedArtistEntityContext(null))
        {
            throw new NotImplementedException();
        }
    }
}