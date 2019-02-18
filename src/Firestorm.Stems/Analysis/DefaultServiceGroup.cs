using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Essentials;

namespace Firestorm.Stems.Analysis
{
    public class DefaultServiceGroup : AnalyzedServiceGroup
    {
        public DefaultServiceGroup(IPropertyAutoMapper propertyAutoMapper = null)
            : base(new SurpremeAnalyzer(propertyAutoMapper ?? new DefaultPropertyAutoMapper(), new DefaultFieldDefinitionAnalyzers()))
        {
        }
    }
}