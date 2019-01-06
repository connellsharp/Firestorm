using Firestorm.Data;
using Firestorm.Engine.Additives.Identifiers;
using Xunit;

namespace Firestorm.Engine.Tests.Additives
{
    public class PrimaryKeyIdentifierInfoTests
    {
        [Fact]
        public void PluralConventionPrimaryKey_GetValue_CorrectForIDProperty()
        {
            var info = new PrimaryKeyIdentifierInfo<Person>(new IdConventionPrimaryKeyFinder());

            var person = new Person { ID = 1 };
            var value = info.GetValue(person);

            Assert.Equal(person.ID, (long)value);
        }

        private class Person
        {
            public long ID { get; set; }
            public string Name { get; set; }
        }
    }
}
