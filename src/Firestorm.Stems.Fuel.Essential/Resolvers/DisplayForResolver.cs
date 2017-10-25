using System;
using System.Collections.Generic;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems.Fuel.Essential.Resolvers
{
    internal class DisplayForResolver : IFieldDefinitionResolver
    {
        public IStemConfiguration Configuration { get; set; }
        public FieldDefinition FieldDefinition { get; set; }

        public void IncludeDefinition<TItem>(EngineImplementations<TItem> implementations)
            where TItem : class
        {
            var defaults = implementations.Defaults;

            foreach (Display displayFor in Enum.GetValues(typeof(Display)))
            {
                if (FieldDefinition.Display >= displayFor)
                {
                    if (!defaults.ContainsKey(displayFor))
                        defaults.Add(displayFor, new List<string>());

                    defaults[displayFor].Add(FieldDefinition.FieldName);
                }
            }
        }
    }
}