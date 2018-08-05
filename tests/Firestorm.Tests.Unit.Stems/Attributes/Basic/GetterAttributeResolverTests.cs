using System;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Basic.Resolvers;
using Firestorm.Stems.Attributes.Definitions;
using Xunit;

namespace Firestorm.Tests.Unit.Stems.Attributes.Basic
{
    public class GetterAttributeResolverTests
    {
        private readonly StemDefinition _definition;
        private readonly GetterAttributeResolver _resolver;

        public GetterAttributeResolverTests()
        {
            _definition = new StemDefinition();

            _resolver = new GetterAttributeResolver(null, Display.FullItem)
            {
                Definition = _definition,
                Attribute = new GetAttribute(),
                ItemType = typeof(Person)
            };
        }

        [Fact]
        public void IncludeMethod_StringGetterWithPersonParam_AddsToDefinition()
        {

            var method = GetType().GetMethod(nameof(GetHash), BindingFlags.Instance | BindingFlags.NonPublic);
            _resolver.IncludeMember(method);

            Assert.True(_definition.FieldDefinitions.ContainsKey("Hash"));
        }

        private string GetHash(Person person)
        {
            return "testHash";
        }

        [Fact]
        public void IncludeMethod_PredicateGetterMethod_AddsToDefinition()
        {

            var method = GetType().GetMethod(nameof(GetHashPredicate), BindingFlags.Instance | BindingFlags.NonPublic);
            _resolver.IncludeMember(method);

            Assert.True(_definition.FieldDefinitions.ContainsKey("HashPredicate"));
        }

        private Expression<Func<Person, string>> GetHashPredicate()
        {
            return p => "testHash";
        }

        public class Person
        {
            public string Salt { get; }
        }
    }
}
