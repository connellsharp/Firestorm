using System;
using System.Linq;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;

namespace Firestorm.Stems.Fuel.Resolving.Analysis
{
    /// <summary>
    /// Analyzes <see cref="FieldDefinition" /> objects to build a <see cref="EngineImplementations{TItem}"/> model from a set of <see cref="IFieldDefinitionResolver"/> implementations.
    /// </summary>
    /// <remarks>
    /// The design with the <see cref="IFieldDefinitionResolver"/> is similar to the <see cref="AttributeAnalyzer"/> used by this very class.
    /// </remarks>
    internal class FieldDefinitionAnalyzer<TItem> : IAnalyzer
        where TItem : class
    {
        public EngineImplementations<TItem> Implementations { get; } = new EngineImplementations<TItem>();

        public void Analyze(Type stemType, IStemConfiguration configuration)
        {
            var attributeAnalyzer = new AttributeAnalyzer();
            attributeAnalyzer.Analyze(stemType, configuration);

            foreach (FieldDefinition definition in attributeAnalyzer.Definition.FieldDefinitions.Values)
            {
                var resolvers = configuration.FeatureSets.SelectMany(fs => fs.GetResolvers<IFieldDefinitionResolver>());

                foreach (IFieldDefinitionResolver resolver in resolvers)
                {
                    try
                    {
                        resolver.Configuration = configuration;
                        resolver.FieldDefinition = definition;

                        resolver.IncludeDefinition(Implementations);
                    }
                    catch (Exception ex)
                    {
                        throw new StemAttributeSetupException("Error setting up the '" + definition.FieldName + "' field with " + resolver.GetType().Name + ".", ex);
                    }
                }
            }
        }
    }
}