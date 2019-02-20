using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Essentials.Factories.Analyzers;
using Firestorm.Stems.Fuel.Analysis;
using Xunit;

namespace Firestorm.Stems.Tests.Fuel
{
    public class SubstemDefinitionResolverTests
    {
        [Fact]
        public void IncludeDefinition_IncludeGetterExpr_AddsFactories()
        {
            var implementations = new EngineImplementations<Person>();

            var analyzer = new SubstemDefinitionAnalyzer();

            Expression<Func<Person, IEnumerable<Person>>> nameExpr = p => p.Friends;

            var definition = new FieldDefinition
            {
                FieldName = "friends",
                SubstemType = typeof(PeopleStem),
                Getter = { Expression = nameExpr }
            };

            analyzer.Analyze(implementations, definition);

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

            var resolver = new SubstemDefinitionAnalyzer();

            var definition = new FieldDefinition
            {
                SubstemType = typeof(PeopleStem)
            };

            Action analyzeMethod = () => resolver.Analyze(implementations, definition);

            Assert.ThrowsAny<StemAttributeSetupException>(analyzeMethod);
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
