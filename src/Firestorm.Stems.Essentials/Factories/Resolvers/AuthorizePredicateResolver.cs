using Firestorm.Stems.Definitions;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems.Fuel.Essential.Resolvers
{
    internal class AuthorizePredicateResolver : IFieldDefinitionResolver
    {
        public IStemConfiguration Configuration { get; set; }
        public FieldDefinition FieldDefinition { get; set; }

        public void IncludeDefinition<TItem>(EngineImplementations<TItem> implementations)
            where TItem : class
        {
            if (FieldDefinition.AuthorizePredicate == null)
                return;

            implementations.AuthorizePredicates.Add(FieldDefinition.FieldName, FieldDefinition.AuthorizePredicate);
        }
    }
}