using System.Collections.Generic;

namespace Firestorm.Stems.Fuel.Resolving.Analysis
{
    internal interface IFieldDefinitionResolverFactory
    {
        IEnumerable<IFieldDefinitionResolver> Resolvers { get; }
    }
}