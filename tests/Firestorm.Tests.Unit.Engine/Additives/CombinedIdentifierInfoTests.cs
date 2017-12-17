using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Identifiers;
using Moq;
using Xunit;

namespace Firestorm.Tests.Unit.Engine.Additives
{
    public class CombinedIdentifierInfoTests
    {
        [Fact]
        public void GetValue_CallsMethod()
        {
            var mock = new Mock<IIdentifierInfo<Person>>();

            var info = new CombinedIdentifierInfo<Person>(new List<IIdentifierInfo<Person>> { mock.Object });

            var person = new Person();
            info.GetValue(person);
            mock.Verify(i => i.GetValue(person));
        }

        [Fact]
        public void SetValue_CallsMethod()
        {
            var mock = new Mock<IIdentifierInfo<Person>>();

            var info = new CombinedIdentifierInfo<Person>(new List<IIdentifierInfo<Person>> { mock.Object });

            var person = new Person();
            info.SetValue(person, "test");
            mock.Verify(i => i.SetValue(person, "test"));
        }

        [Fact]
        public void GetGetterExpression_CallsMethod()
        {
            var mock = new Mock<IIdentifierInfo<Person>>();

            var info = new CombinedIdentifierInfo<Person>(new List<IIdentifierInfo<Person>> { mock.Object });

            var paramExpr = Expression.Parameter(typeof(object));
            info.GetGetterExpression(paramExpr);
            mock.Verify(i => i.GetGetterExpression(paramExpr));
        }

        [Fact]
        public void GetPredicate_CallsMethod()
        {
            var mock = new Mock<IIdentifierInfo<Person>>();

            var info = new CombinedIdentifierInfo<Person>(new List<IIdentifierInfo<Person>> { mock.Object });
            
            info.GetPredicate("test");
            mock.Verify(i => i.GetPredicate("test"));
        }

        [Fact]
        public void GetAlreadyLoadedItem_CallsMethod()
        {
            var mock = new Mock<IIdentifierInfo<Person>>();

            var info = new CombinedIdentifierInfo<Person>(new List<IIdentifierInfo<Person>> { mock.Object });
            
            info.GetAlreadyLoadedItem("test");
            mock.Verify(i => i.GetAlreadyLoadedItem("test"));
        }

        public class Person
        { }
    }
}