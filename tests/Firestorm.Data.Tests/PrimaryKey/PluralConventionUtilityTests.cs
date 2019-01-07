using Firestorm.Data;
using Xunit;

namespace Firestorm.Testing.Data.PrimaryKey
{
    public class PluralConventionUtilityTests
    {
        [Fact]
        public void BasicStrings_GetTableName_Correct()
        {
            Assert.Equal("Dogs", PluralConventionUtility.Pluralize("Dog"));
            Assert.Equal("Morons", PluralConventionUtility.Pluralize("Moron"));
            Assert.Equal("Problems", PluralConventionUtility.Pluralize("Problem"));
        }

        [Fact]
        public void WordsEndingWithY_GetTableName_Correct()
        {
            Assert.Equal("Puppies", PluralConventionUtility.Pluralize("Puppy"));
            Assert.Equal("Doggies", PluralConventionUtility.Pluralize("Doggy"));
            Assert.Equal("Pussies", PluralConventionUtility.Pluralize("Pussy"));
        }

        [Fact]
        public void WordsEndingWithS_GetTableName_Correct()
        {
            Assert.Equal("bosses", PluralConventionUtility.Pluralize("boss"));
            Assert.Equal("glasses", PluralConventionUtility.Pluralize("glass"));
            Assert.Equal("dresses", PluralConventionUtility.Pluralize("dress"));
        }

        [Fact]
        public void WordsEndingWithShChZ_GetTableName_Correct()
        {
            Assert.Equal("Bitches", PluralConventionUtility.Pluralize("Bitch"));
            Assert.Equal("Smashes", PluralConventionUtility.Pluralize("Smash"));
            Assert.Equal("Sexes", PluralConventionUtility.Pluralize("Sex"));
            Assert.Equal("Boxes", PluralConventionUtility.Pluralize("Box"));
            Assert.Equal("Sanchezes", PluralConventionUtility.Pluralize("Sanchez"));
        }

        [Fact]
        public void WordsWithFandV_GetTableName_Correct()
        {
            Assert.Equal("Knives", PluralConventionUtility.Pluralize("Knife"));
            Assert.Equal("Strives", PluralConventionUtility.Pluralize("Strife"));
            Assert.Equal("Calves", PluralConventionUtility.Pluralize("Calf"));
            Assert.Equal("Elves", PluralConventionUtility.Pluralize("Elf"));
            Assert.Equal("Lives", PluralConventionUtility.Pluralize("Life"));
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