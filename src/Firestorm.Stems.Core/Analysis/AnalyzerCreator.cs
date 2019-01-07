using System;

namespace Firestorm.Stems.Analysis
{
    /// <summary>
    /// Creates <see cref="IAnalyzer"/> instances for a supplied analyzer type.
    /// </summary>
    public class AnalyzerCreator : IAnalyzerFactory
    {
        public TAnalyzer GetAnalyzer<TAnalyzer>(Type type, IStemConfiguration configuration)
            where TAnalyzer : IAnalyzer, new()
        {
            var analyzer = new TAnalyzer();
            analyzer.Analyze(type, configuration);
            return analyzer;
        }
    }
}