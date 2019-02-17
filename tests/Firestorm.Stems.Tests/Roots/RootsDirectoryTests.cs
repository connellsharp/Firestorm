using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;
using Firestorm.Stems;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.Combined;
using Firestorm.Stems.Roots.Derive;
using Firestorm.Testing.Models;
using Xunit;

namespace Firestorm.Stems.Tests.Roots
{
    public class RootsDirectoryTests
    {
        [Fact]
        public async Task AddToCollection_EmptyObject_DoesntThrow()
        {
            var namedTypedDictionary = new NamedTypeDictionary();
            namedTypedDictionary.AddType(typeof(TestRoot));

            var directory = new RootsDirectory(new StemsServices(), new DerivedRootStartInfoFactory(namedTypedDictionary), new TestRequestContext());

            var rootCollection = directory.GetCollection("TestRoot"); // normally use the Suffixed type dictionary

            await rootCollection.AddAsync(new { });
        }

        public class TestRoot : Root<Artist>
        {
            public override Type StartStemType { get; } = typeof(TestStem);

            public override Task SaveChangesAsync()
            {
                return Task.FromResult(false);
            }

            public override IQueryable<Artist> GetAllItems()
            {
                return new EnumerableQuery<Artist>(new[]
                {
                    new Artist()
                });
            }

            public override Artist CreateAndAttachItem()
            {
                return new Artist();
            }

            public override void MarkUpdated(Artist item)
            {
                throw new NotImplementedException();
            }

            public override void MarkDeleted(Artist item)
            {
                throw new NotImplementedException();
            }
        }
    }

    public class TestStem : Stem<Artist>
    {
    }

    public class TestRequestContext : IRequestContext
    {
        public IRestUser User { get; set; }

        public event EventHandler OnDispose;
        
        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}