using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Firestorm.Stems.Fuel.Identifiers;
using Xunit;
using System.Linq.Expressions;

namespace Firestorm.Tests.Unit.Stems.Fuel
{
    public class ExpressionAndSetterIdentifierInfoTests
    {
        private readonly Fixture _fixture;

        public ExpressionAndSetterIdentifierInfoTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [Fact]
        public void GetPredicate_SingleIdExpression_DoesntFilterCorrectID()
        {
            var person = _fixture.Create<Person>();
            Expression<Func<Person, int>> idExpr = a => a.ID;
            var identifierInfo = new ExpressionAndSetterIdentifierInfo<Person>(idExpr, null, false);
            
            Expression<Func<Person, bool>> predicate = identifierInfo.GetPredicate(person.ID.ToString());

            var filtered = (new[] { person }).AsQueryable().Where(predicate);
            
            Assert.Equal(1, filtered.Count());
        }

        [Fact]
        public void GetValue_SingleIdExpression_Correct()
        {
            var person = _fixture.Create<Person>();
            Expression<Func<Person, int>> idExpr = a => a.ID;
            var identifierInfo = new ExpressionAndSetterIdentifierInfo<Person>(idExpr, null, false);
            
            var result = identifierInfo.GetValue(person);
            
            Assert.Equal(person.ID, result);
        }

        [Fact]
        public void GetValue_MultiIdExpression_Correct()
        {
            var person = _fixture.Create<Person>();
            Expression<Func<Person, IEnumerable<string>>> namesExpr = a => a.Names;
            var identifierInfo = new ExpressionAndSetterIdentifierInfo<Person>(namesExpr, null, true);
            
            var result = identifierInfo.GetValue(person);
            
            var enumerable = Assert.IsAssignableFrom<IEnumerable<string>>(result);
            Assert.True(person.Names.SequenceEqual(enumerable));
        }

        [Fact]
        public void SetValue_SingleIdExpressionNullSetterAction_UpdatesValue()
        {
            var person = _fixture.Create<Person>();
            Expression<Func<Person, int>> idExpr = a => a.ID;
            var identifierInfo = new ExpressionAndSetterIdentifierInfo<Person>(idExpr, null, false);

            identifierInfo.SetValue(person, "123");

            Assert.Equal(123, person.ID);
        }

        [Fact]
        public void SetValue_SingleIdExpressionSetterActionAddsOne_UpdatesValueWithAddedOne()
        {
            var person = _fixture.Create<Person>();
            Expression<Func<Person, int>> idExpr = a => a.ID;
            Action<Person, string> setter = (p, s) => p.ID = int.Parse(s) + 1;
            var identifierInfo = new ExpressionAndSetterIdentifierInfo<Person>(idExpr, setter, false);

            identifierInfo.SetValue(person, "123");

            Assert.Equal(124, person.ID);
        }

        public class Person
        {
            public int ID { get; set; }
            public IEnumerable<string> Names { get; set; }
        }
    }
}
