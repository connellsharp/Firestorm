using System.Collections.Generic;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Fuel.Essential;
using Firestorm.Stems.Fuel.Substems;
using Firestorm.Stems.Naming;

namespace Firestorm.Stems.Roots
{
    public class DefaultStemConfiguration : IStemConfiguration
    {
        public NamingConventionSwitcher NamingConventionSwitcher { get; set; } = new DefaultNamingConventionSwitcher();

        public IDependencyResolver DependencyResolver { get; set; } = null;

        public IPropertyAutoMapper AutoPropertyMapper { get; set; } = new DefaultPropertyAutoMapper();

        public IEnumerable<IStemsFeatureSet> FeatureSets { get; set; } = new List<IStemsFeatureSet> { new EssentialFeatureSet(), new SubstemsFeatureSet() };
    }
}