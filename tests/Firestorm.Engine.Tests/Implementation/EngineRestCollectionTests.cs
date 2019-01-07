using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Fields;
using Firestorm.Tests.Unit;
using Xunit;

namespace Firestorm.Engine.Tests.Implementation
{
    public class EngineRestCollectionTests
    {
        private readonly Fixture _fixture;

        public EngineRestCollectionTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [Theory]
        [InlineData(20, 10, true)]
        [InlineData(10, 20, false)]
        [InlineData(10, 10, false)]
        [InlineData(11, 10, true)]
        public async Task Get_PageLimit_ShouldHaveNextPage(int collectionSize, int pageSize, bool shouldHaveNextPage)
        {
            //_fixture.FreezeMock<IAuthorizationChecker<Person>>(m =>
            //{
            //    m.SetupIgnoreArgs(a => a.CanGetField(null, null)).Returns(true);
            //});

            _fixture.FreezeMock<IFieldProvider<Person>>(m =>
            {
                m.SetupIgnoreArgs(a => a.FieldExists(null)).Returns(true);
                m.SetupIgnoreArgs(a => a.GetReader(null)).Returns(new ExpressionFieldReader<Person, string>(p => p.Name));
            });

            var repo = _fixture.CreateMany<Person>(collectionSize).ToList();

            _fixture.Inject(repo.AsQueryable());

            var query = new TestCollectionQuery
            {
                PageInstruction = new PageInstruction { Size = pageSize }
            };

            var collection = _fixture.Create<EngineRestCollection<Person>>();

            var data = await collection.QueryDataAsync(query);

            Assert.Equal(shouldHaveNextPage, data.PageDetails.HasNextPage);
        }

        public class Person
        {
            public string Name { get; set; }
        }
    }
}