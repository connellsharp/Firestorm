using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Additives.Writers;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Repositories;
using Moq;
using Xunit;

namespace Firestorm.Engine.Tests.Implementation
{
    public class EngineRestScalarTests
    {
        private readonly Fixture _fixture;

        public EngineRestScalarTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [Fact]
        public async Task Get_CantGetField_ThrowsNotAuthorized()
        {
            _fixture.FreezeMock<IAuthorizationChecker<Person>>(m =>
            {
                m.SetupIgnoreArgs(a => a.CanGetField(null, null)).Returns(false);
            });

            var scalar = _fixture.Create<EngineRestScalar<Person>>();

            await Assert.ThrowsAsync<NotAuthorizedForFieldException>(async delegate
            {
                await scalar.GetAsync();
            });
        }

        [Fact]
        public async Task Get_HappyPath_CorrectName()
        {
            _fixture.FreezeMock<IAuthorizationChecker<Person>>(m =>
            {
                m.SetupIgnoreArgs(a => a.CanGetField(null, null)).Returns(true);
            });

            _fixture.Inject<IFieldReader<Person>>(new ExpressionFieldReader<Person,string>(p => p.Name));

            var person = _fixture.Create<Person>();
            _fixture.Inject(new[] { person }.SingleDefferred());

            _fixture.Relay<IEngineRepository<Person>, QueryableSingleRepository<Person>>();

            var scalar = _fixture.Create<EngineRestScalar<Person>>();

            var obj = await scalar.GetAsync();
            string str = Assert.IsType<string>(obj);
            Assert.Equal(person.Name, str);
        }

        [Fact]
        public async Task Edit_CantGetField_ThrowsNotAuthorized()
        {
            _fixture.FreezeMock<IAuthorizationChecker<Person>>(m =>
            {
                m.SetupIgnoreArgs(a => a.CanEditField(null, null)).Returns(false);
            });

            var scalar = _fixture.Create<EngineRestScalar<Person>>();

            await Assert.ThrowsAsync<NotAuthorizedForFieldException>(async delegate
            {
                await scalar.EditAsync("test");
            });
        }

        [Fact]
        public async Task Edit_HappyPath_EditsValue()
        {
            _fixture.FreezeMock<IAuthorizationChecker<Person>>(m =>
            {
                m.SetupIgnoreArgs(a => a.CanEditField(null, null)).Returns(true);
            });

            _fixture.Inject<IFieldWriter<Person>>(new PropertyExpressionFieldWriter<Person, string>(p => p.Name));

            var person = _fixture.Create<Person>();
            _fixture.Inject(person);

            var scalar = _fixture.Create<EngineRestScalar<Person>>();

            var newValue = _fixture.Create<string>();
            await scalar.EditAsync(newValue);

            Assert.Equal(newValue, person.Name);
        }

        public class Person
        {
            public string Name { get; set; }
        }
    }
}