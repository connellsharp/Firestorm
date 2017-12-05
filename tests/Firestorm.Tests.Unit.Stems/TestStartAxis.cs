using System;
using System.Collections.Generic;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Fuel.Essential;
using Firestorm.Stems.Fuel.Substems;
using Firestorm.Stems.Naming;

namespace Firestorm.Tests.Unit.Stems
{
    internal class TestStartAxis : IAxis
    {
        public IRestUser User { get; }

        public IStemConfiguration Configuration { get; } = new TestStemConfiguration();

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }

        private class TestStemConfiguration : IStemConfiguration
        {
            public NamingConventionSwitcher NamingConventionSwitcher { get; } = new DefaultNamingConventionSwitcher();

            public IDependencyResolver DependencyResolver { get; }

            public IPropertyAutoMapper AutoPropertyMapper { get; } = new DefaultPropertyAutoMapper();

            public IEnumerable<IStemsFeatureSet> FeatureSets { get; } = new List<IStemsFeatureSet> { new EssentialFeatureSet(), new SubstemsFeatureSet() };

            public IAnalyzerFactory AnalyzerCache { get; } = new AnalyzerCache();
        }
    }
}