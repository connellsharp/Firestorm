using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AutoFixture;
using Firestorm.Stems.Fuel.Identifiers;
using Xunit;

namespace Firestorm.Tests.Unit.Stems.Fuel
{
    public class PredicateMethodIdentifierInfoTests
    {
        private readonly PredicateMethodIdentifierInfo<Person> _info;
        private readonly Fixture _fixture;

        public PredicateMethodIdentifierInfoTests()
        {
            _fixture = new Fixture();

            MethodInfo getNamePredicateMethod = GetType().GetMethod(nameof(GetNamePredicate), BindingFlags.Static | BindingFlags.NonPublic);
            _info = new PredicateMethodIdentifierInfo<Person>(getNamePredicateMethod, SetName);
        }

        private void SetName(Person person, string name)
        {
            person.Name = name;
        }

        private static Expression<Func<Person, bool>> GetNamePredicate(string name)
        {
            return p => p.Name == name;
        }

        public class Person
        {
            public string Name { get; set; }
        }

        [Fact]
        public void GetPredicate_TestPersonArray_FindsInArray()
        {
            var person = _fixture.Create<Person>();

            var predicate = _info.GetPredicate(person.Name);

            var person2 = new[] { person }.AsQueryable().Where(predicate).Single();

            Assert.Equal(person, person2);
        }

        [Fact]
        public void SetValue_TestPerson_Correct()
        {
            var person = _fixture.Create<Person>();
            string newName = _fixture.Create<string>();

            _info.SetValue(person, newName);

            Assert.Equal(newName, person.Name);
        }
    }
}