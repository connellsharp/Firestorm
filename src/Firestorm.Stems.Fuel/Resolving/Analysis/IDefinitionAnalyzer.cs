using Firestorm.Stems.Definitions;

namespace Firestorm.Stems.Fuel.Resolving.Analysis
{   
    /// <summary>
    /// Adds to a <see cref="EngineImplementations{TItem}"/> using information from a definition object, such as <see cref="FieldDefinition"/>.
    /// </summary>
    /// <remarks>
    /// Similar design to the <see cref="IAnalyzer"/>, but has generic method and type param.
    /// </remarks>
    public interface IDefinitionAnalyzer<in TDefinition>
    {
        void Analyze<TItem>(EngineImplementations<TItem> implementations, TDefinition definition) 
            where TItem : class;
    }
}