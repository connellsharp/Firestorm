using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Attributes.Definitions;

namespace Firestorm.Stems.Fuel.Resolving.Analysis
{
    /// <summary>
    /// Adds to a <see cref="EngineImplementations{TItem}"/> with information about a <see cref="Attributes.Definitions.FieldDefinition"/>.
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