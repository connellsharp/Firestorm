using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;

namespace Firestorm.Stems.Fuel.Resolving.Analysis
{
    /// <summary>
    /// Adds to a <see cref="EngineImplementations{TItem}"/> with information about a <see cref="Definitions.FieldDefinition"/>.
    /// </summary>
    /// <remarks>
    /// Similar design to the <see cref="IAttributeResolver"/>.
    /// </remarks>
    public interface IFieldDefinitionResolver : IAnalysisResolver
    {
        FieldDefinition FieldDefinition { set; } // TODO: this is kinda the opposite approach to IAttributeResolver

        void IncludeDefinition<TItem>(EngineImplementations<TItem> implementations)
            where TItem : class;
    }
}