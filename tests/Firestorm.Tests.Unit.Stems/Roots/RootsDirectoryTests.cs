using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.Combined;
using Firestorm.Stems.Roots.Derive;
using Xunit;

namespace Firestorm.Tests.Unit.Stems.Roots
{
    public class RootsDirectoryTests
    {
        [Fact]
        public async Task AddToCollection_EmptyObject_DoesntThrow()
        {
            var namedTypedDictionary = new NamedTypeDictionary();
            namedTypedDictionary.AddType(typeof(TestRoot));

            var directory = new RootsDirectory(new DefaultStemConfiguration(), new DerivedRootStartInfoFactory(namedTypedDictionary), new TestRootRequest());

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

            public override void MarkDeleted(Artist item)
            {
                throw new NotImplementedException();
            }
        }
    }

    public class TestStem : Stem<Artist>
    {
        public override bool CanAddItem()
        {
            return true;
        }
    }

    public class TestRootRequest : IRootRequest
    {
        public IRestUser User { get; }

        public event EventHandler OnDispose;
    }
}