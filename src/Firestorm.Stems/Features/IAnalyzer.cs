using System;

namespace Firestorm.Stems
{
    public interface IAnalyzer
    {
        /* This interface was from .Attributes but is required in IStemConfiguration
         * Somewhere, this whole analzyer/resolver thing should be refactored.
         */

        void Analyze(Type stemType, IStemConfiguration configuration);
    }
}