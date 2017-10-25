using System;

namespace Firestorm.Stems.Attributes.Analysis
{
    public interface IAnalyzer
    {
        void Analyze(Type stemType, IStemConfiguration configuration);
    }
}