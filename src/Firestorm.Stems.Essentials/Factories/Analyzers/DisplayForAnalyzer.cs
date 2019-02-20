using System;
using System.Collections.Generic;
using Firestorm.Data;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Fuel.Analysis;

namespace Firestorm.Stems.Essentials.Factories.Analyzers
{
    internal class DisplayForAnalyzer : IDefinitionAnalyzer<FieldDefinition>
    {
        public IStemsCoreServices Configuration { get; set; }

        public void Analyze<TItem>(EngineImplementations<TItem> implementations, FieldDefinition definition) 
            where TItem : class
        {
            var defaults = implementations.Defaults;

            foreach (Display displayFor in Enum.GetValues(typeof(Display)))
            {
                var display = definition.Display ??
                              GetDefaultDisplayFromName(typeof(TItem).Name, definition.FieldName);
                
                if (display >= displayFor)
                {
                    if (!defaults.ContainsKey(displayFor))
                        defaults.Add(displayFor, new List<string>());

                    defaults[displayFor].Add(definition.FieldName);
                }
            }
        }

        private static Display GetDefaultDisplayFromName(string itemName, string fieldName)
        {
            bool isId = IdConventionPrimaryKeyFinder.GetPossibleIdNames(itemName).Contains(fieldName);
            return isId ? Display.Nested : Display.FullItem;
        }
    }
}