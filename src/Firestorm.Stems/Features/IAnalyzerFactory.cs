using System;

namespace Firestorm.Stems
{
    /// <summary>
    /// Creates <see cref="IAnalyzer"/> instances for a supplied analyzer type.
    /// </summary>
    public interface IAnalyzerFactory
    {
        TAnalyzer GetAnalyzer<TAnalyzer>(Type stemType, IStemConfiguration configuration)
            where TAnalyzer : IAnalyzer, new();
    }
}