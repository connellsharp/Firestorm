using Firestorm.Data;
using Xunit;

namespace Firestorm.Tests.Unit.Data.PrimaryKey
{
    public class PluralConventionUtilityTests
    {
        [Fact]
        public void BasicStrings_GetTableName_Correct()
        {
            Assert.Equal("Dogs", PluralConventionUtility.Pluralize("Dog"));
            Assert.Equal("Morons", PluralConventionUtility.Pluralize("Moron"));
            Assert.Equal("Puppies", PluralConventionUtility.Pluralize("Puppy"));
            Assert.Equal("Bitches", PluralConventionUtility.Pluralize("Bitch"));
            Assert.Equal("Smashes", PluralConventionUtility.Pluralize("Smash"));
        }

        [Fact]
        public void ComplexPlurals_GetTableName_Correct()
        {
            Assert.Equal("People", PluralConventionUtility.Pluralize("Person"));
            Assert.Equal("Teeth", PluralConventionUtility.Pluralize("Tooth"));
        }

        [Fact]
        public void Type_GetTableName_Correct()
        {
            Assert.Equal("MyTestEntities", PluralConventionUtility.GetTableName<MyTestEntity>());
        }

        public class MyTestEntity
        { }
    }
}