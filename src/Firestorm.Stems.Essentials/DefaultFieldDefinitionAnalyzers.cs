using System.Collections;
using System.Collections.Generic;
using Firestorm.Stems.Essentials.Factories.Resolvers;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems.Essentials
{
    public class DefaultFieldDefinitionAnalyzers : IEnumerable<IFieldDefinitionAnalyzer>
    {
        private readonly List<IFieldDefinitionAnalyzer> _list;

        public DefaultFieldDefinitionAnalyzers()
        {
            _list = new List<IFieldDefinitionAnalyzer>
            {
                new ExpressionOnlyDefinitionAnalyzer(),
                new RuntimeMethodDefinitionAnalyzer(),
                new AuthorizePredicateAnalyzer(),
                new DescriptionAnalyzer(),
                new DisplayForAnalyzer(),
                new SubstemDefinitionAnalyzer()
            };
        }
        
        public IEnumerator<IFieldDefinitionAnalyzer> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}