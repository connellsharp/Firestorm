using System.Data.Linq.Mapping;
using Firestorm.Data.EntityFramework;
using Xunit;

namespace Firestorm.Tests.Entities.PrimaryKey
{
    public class PrimaryKeyUtilityTests
    {
        [Fact]
        public void Writer_GetByPrimaryKey_DoesntThrow()
        {
            var keyFinder = new EntityPrimaryKeyFinder();
            var pi = keyFinder.GetPrimaryKeyInfo<MyTestEntity>();
            Assert.Equal("PrimaryKey", pi.Name);
        }

        [Fact]
        public void BasicStrings_GetTableName_Correct()
        {
            Assert.Equal("Dogs", EntityPrimaryKeyFinder.Pluralize("Dog"));
            Assert.Equal("Morons", EntityPrimaryKeyFinder.Pluralize("Moron"));
            Assert.Equal("Puppies", EntityPrimaryKeyFinder.Pluralize("Puppy"));
            Assert.Equal("Bitches", EntityPrimaryKeyFinder.Pluralize("Bitch"));
            Assert.Equal("Smashes", EntityPrimaryKeyFinder.Pluralize("Smash"));
        }

        [Fact]
        public void ComplexPlurals_GetTableName_Correct()
        {
            Assert.Equal("People", EntityPrimaryKeyFinder.Pluralize("Person"));
            Assert.Equal("Teeth", EntityPrimaryKeyFinder.Pluralize("Tooth"));
        }

        [Fact]
        public void Type_GetTableName_Correct()
        {
            var keyFinder = new EntityPrimaryKeyFinder();
            Assert.Equal("MyTestEntities", keyFinder.GetTableName<MyTestEntity>());
        }
    }

    public class MyTestEntity
    {
        public string NotPrimary { get; set; }

        [Column(IsPrimaryKey = true)]
        public int PrimaryKey { get; set; }
    }
}