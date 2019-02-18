using Firestorm.Stems.Analysis;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Essentials;

namespace Firestorm.Stems.Tests
{
    internal class TestStemsServices : StemsServices
    {
        public TestStemsServices()
        {
            AutoPropertyMapper = new DefaultPropertyAutoMapper();

            ServiceGroup =
                new AnalyzedServiceGroup(new SurpremeAnalyzer(new DefaultFieldDefinitionAnalyzers()));
        }
    }
}