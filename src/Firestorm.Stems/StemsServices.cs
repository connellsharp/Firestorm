using System.Collections.Generic;
using System.Linq;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems
{
    public class StemsServices : IStemsCoreServices
    {
        public IDependencyResolver DependencyResolver { get; set; }

        public IPropertyAutoMapper AutoPropertyMapper { get; set; }

        public IImplementationResolver ImplementationResolver { get; set; }
        
        public IEnumerable<IFieldDefinitionAnalyzer> Analyzers { get; set; } = Enumerable.Empty<IFieldDefinitionAnalyzer>();
    }
}