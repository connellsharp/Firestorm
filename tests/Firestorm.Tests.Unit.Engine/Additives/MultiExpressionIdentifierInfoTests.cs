using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Engine.Additives.Identifiers;
using Xunit;

namespace Firestorm.Tests.Unit.Engine.Additives
{
    public class MultiExpressionIdentifierInfoTests
    {
        [Theory]
        [InlineData("bob", true)]
        [InlineData("bilbo", false)]
        public void GetPredicate_GetPredicateWithACorrectName_IncludesInPredicateFilter(string name, bool shouldAllowThroughFilter)
        {
            var identifierInfo = new MultiExpressionIdentifierInfo<MultiNamedThing, string>(mnt => mnt.Names);

            var thing = new MultiNamedThing
            {
                Names = new List<string> { "bill", "ben", "bob" }
            };
            
            Expression<Func<MultiNamedThing, bool>> predicate = identifierInfo.GetPredicate(name);

            var resultCount = new[] { thing }.AsQueryable().Where(predicate).Count();
            
            Assert.Equal(shouldAllowThroughFilter ? 1 : 0, resultCount);
        }

        private class MultiNamedThing
        {
            public IEnumerable<string> Names { get; set; }
        }
    }
}
