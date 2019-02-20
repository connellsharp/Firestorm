using Firestorm.Stems.Definitions;
using Firestorm.Stems.Essentials.Factories.Analyzers;
using Firestorm.Stems.Fuel.Analysis;
using Firestorm.Testing.Models;
using Xunit;

namespace Firestorm.Stems.Tests.Analyzers
{
    public class DisplayNestingAnalyzerTests
    {
        [Fact]
        public void DisplayFullItem_AppearsInFullItem()
        {
            var definition = new FieldDefinition
            {
                FieldName = "Test",
                Display = Display.FullItem
            };
            
            var analyzer = new DisplayNestingAnalyzer();
            var implementations = new EngineImplementations<Artist>();
            analyzer.Analyze(implementations, definition);
            
            Assert.Contains("Test", implementations.Defaults[Display.FullItem]);
        }
        
        [Fact]
        public void DisplayHidden_DoesNotAppearInFullItem()
        {
            var definition = new FieldDefinition
            {
                FieldName = "Test",
                Display = Display.Hidden
            };
            
            var analyzer = new DisplayNestingAnalyzer();
            var implementations = new EngineImplementations<Artist>();
            analyzer.Analyze(implementations, definition);
            
            Assert.DoesNotContain(Display.FullItem, implementations.Defaults.Keys);
        }
        
        [Fact]
        public void DisplayNestedMany_AppearsInFullItem()
        {
            var definition = new FieldDefinition
            {
                FieldName = "Test",
                Display = Display.NestedMany
            };
            
            var analyzer = new DisplayNestingAnalyzer();
            var implementations = new EngineImplementations<Artist>();
            analyzer.Analyze(implementations, definition);
            
            Assert.Contains("Test", implementations.Defaults[Display.FullItem]);
        }
        
        [Fact]
        public void DisplayUnspecifiedAndNameNotId_DoesNotAppearInNested()
        {
            var definition = new FieldDefinition
            {
                FieldName = "Test"
            };
            
            var analyzer = new DisplayNestingAnalyzer();
            var implementations = new EngineImplementations<Artist>();
            analyzer.Analyze(implementations, definition);
            
            Assert.DoesNotContain(Display.Nested, implementations.Defaults.Keys);
        }
        
        [Fact]
        public void DisplayUnspecifiedAndNameId_AppearsInNested()
        {
            var definition = new FieldDefinition
            {
                FieldName = "Id"
            };
            
            var analyzer = new DisplayNestingAnalyzer();
            var implementations = new EngineImplementations<Artist>();
            analyzer.Analyze(implementations, definition);
            
            Assert.Contains("Id", implementations.Defaults[Display.Nested]);
        }
    }
}