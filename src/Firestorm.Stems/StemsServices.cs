using System.Linq;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems
{
    public class StemsServices : IStemsCoreServices
    {
        public IDependencyResolver DependencyResolver { get; set; }

        public IPropertyAutoMapper AutoPropertyMapper { get; set; }
        
        public IServiceGroup ServiceGroup { get; set; }
        
        public IDefinitionAnalyzers DefinitionAnalyzers { get; set; }
    }
}