using System.Data.Linq.Mapping;
using Firestorm.Engine.EntityFramework.PrimaryKey;
using Xunit;

namespace Firestorm.Tests.Entities.PrimaryKey
{
    public class PrimaryKeyUtilityTests
    {
        [Fact]
        public void Writer_GetByPrimaryKey_DoesntThrow()
        {
            var pi = PrimaryKeyUtility.GetPrimaryKeyInfo<MyTestEntity>();
            Assert.Equal("PrimaryKey", pi.Name);
        }

        [Fact]
        public void BasicStrings_GetTableName_Correct()
        {
            Assert.Equal("Dogs", PrimaryKeyUtility.Pluralize("Dog"));
            Assert.Equal("Morons", PrimaryKeyUtility.Pluralize("Moron"));
            Assert.Equal("Puppies", PrimaryKeyUtility.Pluralize("Puppy"));
            Assert.Equal("Bitches", PrimaryKeyUtility.Pluralize("Bitch"));
            Assert.Equal("Smashes", PrimaryKeyUtility.Pluralize("Smash"));
        }

        [Fact]
        public void ComplexPlurals_GetTableName_Correct()
        {
            Assert.Equal("People", PrimaryKeyUtility.Pluralize("Person"));
            Assert.Equal("Teeth", PrimaryKeyUtility.Pluralize("Tooth"));
        }

        [Fact]
        public void Type_GetTableName_Correct()
        {
            Assert.Equal("MyTestEntities", PrimaryKeyUtility.GetTableName<MyTestEntity>());
        }
    }

    public class MyTestEntity
    {
        public string NotPrimary { get; set; }

        [Column(IsPrimaryKey = true)]
        public int PrimaryKey { get; set; }
    }
}