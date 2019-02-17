using Firestorm.Stems.Definitions;

namespace Firestorm.Stems.Fuel.Resolving.Analysis
{
    /// <summary>
    /// Adds to a <see cref="EngineImplementations{TItem}"/> with information about a <see cref="Definitions.FieldDefinition"/>.
    /// </summary>
    /// <remarks>
    /// Similar design to the <see cref="IAnalyzer"/>, but has generic method.
    /// </remarks>
    public interface IFieldDefinitionAnalyzer
    {
        void Analyze<TItem>(EngineImplementations<TItem> implementations, FieldDefinition definition) 
            where TItem : class;
    }
}