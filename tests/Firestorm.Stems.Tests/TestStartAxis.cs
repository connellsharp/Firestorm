using System;
using System.Collections.Generic;
using Firestorm.Stems;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Essentials;

namespace Firestorm.Stems.Tests
{
    internal class TestStartAxis : IAxis
    {
        public IRestUser User { get; }

        public IStemsCoreServices Configuration { get; } = new TestStemsServices();

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }

        private class TestStemsServices : IStemsCoreServices
        {
            public IDependencyResolver DependencyResolver { get; }

            public IPropertyAutoMapper AutoPropertyMapper { get; } = new DefaultPropertyAutoMapper();

            public IEnumerable<IStemsFeatureSet> FeatureSets { get; } = new List<IStemsFeatureSet> { new EssentialFeatureSet(), new SubstemsFeatureSet() };

            public IAnalyzerFactory AnalyzerCache { get; } = new AnalyzerCache();
        }
    }
}