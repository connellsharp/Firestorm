using System;
using System.Collections.Generic;
using Firestorm.Data;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Fuel.Analysis;

namespace Firestorm.Stems.Essentials.Factories.Analyzers
{
    internal class DisplayNestingAnalyzer : IDefinitionAnalyzer<FieldDefinition>
    {
        public void Analyze<TItem>(EngineImplementations<TItem> implementations, FieldDefinition definition) 
            where TItem : class
        {
            Dictionary<Display, List<string>> defaults = implementations.Defaults;

            foreach (Display d in Enum.GetValues(typeof(Display)))
            {
                Display display = definition.Display ??
                                  GetDefaultDisplayFromName(typeof(TItem).Name, definition.FieldName);
                
                if (display >= d)
                {
                    if (!defaults.ContainsKey(d))
                        defaults.Add(d, new List<string>());

                    defaults[d].Add(definition.FieldName);
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