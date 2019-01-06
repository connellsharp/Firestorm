using System.Collections.Generic;
using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Fuel.Essential;
using Firestorm.Stems.Fuel.Substems;

namespace Firestorm.Stems.Roots
{
    public class DefaultStemConfiguration : IStemConfiguration
    {
        public IDependencyResolver DependencyResolver { get; set; } = null;

        public IPropertyAutoMapper AutoPropertyMapper { get; set; } = new DefaultPropertyAutoMapper();

        public IEnumerable<IStemsFeatureSet> FeatureSets { get; set; } = new List<IStemsFeatureSet> { new EssentialFeatureSet(), new SubstemsFeatureSet() };

        public IAnalyzerFactory AnalyzerCache { get; } = new AnalyzerCache();
    }
}