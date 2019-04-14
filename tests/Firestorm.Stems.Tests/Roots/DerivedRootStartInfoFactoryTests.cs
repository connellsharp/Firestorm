using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.Derive;
using Firestorm.Stems.Tests.Attributes;
using Firestorm.Testing.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace Firestorm.Stems.Tests.Roots
{
    public class DerivedRootStartInfoFactoryTests
    {
        [Fact]
        public async Task GetRootStartInfo_StemType_Expected()
        {
            var stemsServices = new StemsServices();

            var factory = new DerivedRootStartInfoFactory(new NestedTypeGetter(GetType()));

            var startInfo = factory.Get(stemsServices, "Test");

            startInfo.GetStemType().Should().Be<TestStem>();
        }
        
        [Fact]
        public async Task GetStemTypes_ContainsExpected()
        {
            var stemsServices = new StemsServices();

            var sut = new DerivedRootStartInfoFactory(new NestedTypeGetter(GetType()));

            var stemTypes = sut.GetStemTypes(stemsServices);

            stemTypes.Should().ContainSingle().Which.Should().Be<TestStem>();
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