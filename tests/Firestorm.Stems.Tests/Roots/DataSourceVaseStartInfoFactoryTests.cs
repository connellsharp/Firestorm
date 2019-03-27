using System;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Stems.Roots;
using Xunit;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Testing.Models;
using FluentAssertions;
using Moq;

namespace Firestorm.Stems.Tests.Roots
{
    public class DataSourceVaseStartInfoFactoryTests
    {
        private readonly DataSourceVaseStartInfoFactory _sut;
        private readonly Mock<IDataSource> _dataSourceMock;

        public DataSourceVaseStartInfoFactoryTests()
        {
            _dataSourceMock = new Mock<IDataSource>();

            var typeGetter = new NestedTypeGetter(GetType());

            _sut = new DataSourceVaseStartInfoFactory(_dataSourceMock.Object, typeGetter,
                DataSourceRootAttributeBehavior.UseAllStemsExceptDisallowed);
        }

        [Fact]
        public async Task GetStemTypes_ContainsValidStemTypes()
        {
            var types = _sut.GetStemTypes(new TestStemsServices());

            types.Should().Contain(new[] {typeof(TestStem)});
        }

        [Fact]
        public async Task GetStemTypes_DoesNotContainInvalidStemTypes()
        {
            var types = _sut.GetStemTypes(new TestStemsServices());

            types.Should().NotContain(new[] {typeof(StemNamedIncorrectly), typeof(JustCalledAStem), typeof(DisallowedStem)});
        }

        [Fact]
        public async Task ValidTestStemTypeString_GetType_TestStem()
        {
            var services = new TestStemsServices();

            _sut.GetStemTypes(new TestStemsServices());
            var type = _sut.Get(services, "Test").GetStemType();

            type.Should().Be<TestStem>();
        }

        [Fact]
        public async Task GetDataSource_ReturnsDataSourceFromFactory()
        {
            var services = new TestStemsServices();

            var dataSource = _sut.Get(services, "Test").GetDataSource();
            
            dataSource.Should().Be(_dataSourceMock.Object);
        }

        public class TestStem : Stem<Artist>
        {
        }

        [NoDataSourceRoot]
        public class DisallowedStem : Stem<Artist>
        {
        }

        public class StemNamedIncorrectly : Stem<Artist>
        {
        }

        public class JustCalledAStem
        {
        }
    }
}