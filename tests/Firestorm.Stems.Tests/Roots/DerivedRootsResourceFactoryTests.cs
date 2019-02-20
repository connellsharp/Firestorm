using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.Derive;
using Firestorm.Testing.Models;
using Moq;
using Xunit;

namespace Firestorm.Stems.Tests.Roots
{
    public class DerivedRootsResourceFactoryTests
    {
        [Fact]
        public async Task GetStartResource_MockRootFactory_CallsGetStartResource()
        {
            var stemConfig = new StemsServices();

            var factory = new DerivedRootsResourceFactory
            {
                RootTypeGetter = new NestedTypeGetter(GetType())
            };

            factory.GetStemTypes(stemConfig);

            var startResource = factory.GetStartResource(stemConfig, new TestRequestContext());

            var startDirectory = Assert.IsAssignableFrom<IRestDirectory>(startResource);
            var info = await startDirectory.GetInfoAsync();

            Assert.Equal(1, info.Resources.Count());
            Assert.Equal("Test", info.Resources.Single().Name);
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
}