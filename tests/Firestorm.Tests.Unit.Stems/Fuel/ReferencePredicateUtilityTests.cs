using System;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.AutoMoq;
using Firestorm.Stems.Fuel.Identifiers;
using Xunit;

namespace Firestorm.Tests.Unit.Stems.Fuel
{
    public class ReferencePredicateUtilityTests
    {
        private readonly Fixture _fixture;

        public ReferencePredicateUtilityTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [Fact]
        public void CombinePredicates_PredicateWithNameFromList_FindsItemWithName()
        {
            var list = _fixture.CreateMany<Thing>(5).ToList();
            var firstItemsName = list.First().Name;

            Expression<Func<Thing, bool>> falsePredicate = t => false;
            Expression<Func<Thing, bool>> idPredicate = t => t.ID == -123;
            Expression<Func<Thing, bool>> namePredicate = t => t.Name == firstItemsName;

            var combinedPredicate = PredicateUtility.CombinePredicates(falsePredicate, idPredicate, namePredicate);

            var foundItem = list.AsQueryable().Where(combinedPredicate).SingleOrDefault();

            Assert.Equal(firstItemsName, foundItem.Name);
        }
    }

    internal class Thing
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
