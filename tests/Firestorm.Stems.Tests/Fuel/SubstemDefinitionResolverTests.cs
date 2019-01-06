using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.AutoMoq;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using Firestorm.Stems.Fuel.Substems;
using Firestorm.Stems.Roots;
using Xunit;

namespace Firestorm.Stems.Tests.Fuel
{
    public class SubstemDefinitionResolverTests
    {
        private readonly Fixture _fixture;

        public SubstemDefinitionResolverTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [Fact]
        public void IncludeDefinition_IncludeGetterExpr_AddsFactories()
        {
            var implementations = new EngineImplementations<Person>();

            var resolver = new SubstemDefinitionResolver();
            resolver.Configuration = new DefaultStemConfiguration();

            Expression<Func<Person, IEnumerable<Person>>> nameExpr = p => p.Friends;

            resolver.FieldDefinition = new FieldDefinition
            {
                FieldName = "friends",
                SubstemType = typeof(PeopleStem),
                Getter = { Expression = nameExpr }
            };

            resolver.IncludeDefinition(implementations);

            Assert.Equal(1, implementations.FullResourceFactories.Count);
            Assert.Equal(1, implementations.ReaderFactories.Count);
            Assert.Equal(1, implementations.WriterFactories.Count);

            //var reader = implementations.ReaderFactories.First().Value.Get(null);

            //var body = reader.GetSelectExpression(nameExpr.Parameters[0]);
            //Assert.Equal(nameExpr.ResourceBody, body);
        }

        [Fact]
        public void IncludeDefinition_NoGetterExpression_ThrowsSetupException()
        {
            var implementations = new EngineImplementations<Person>();

            var resolver = new SubstemDefinitionResolver();
            resolver.Configuration = new DefaultStemConfiguration();

            resolver.FieldDefinition = new FieldDefinition
            {
                SubstemType = typeof(PeopleStem)
            };

            Action include = () => resolver.IncludeDefinition(implementations);

            Assert.ThrowsAny<StemAttributeSetupException>(include);
        }

        public class PeopleStem : Stem<Person>
        { }

        public class Person
        {
            public string Name { get; set; }

            public IEnumerable<Person> Friends { get; set; }
        }
    }
}
