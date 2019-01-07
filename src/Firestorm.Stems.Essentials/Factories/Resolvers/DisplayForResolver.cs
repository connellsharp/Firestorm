using System;
using System.Collections.Generic;
using Firestorm.Data;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems.Essentials.Factories.Resolvers
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
                var display = FieldDefinition.Display ??
                              GetDefaultDisplayFromName(typeof(TItem).Name, FieldDefinition.FieldName);
                
                if (display >= displayFor)
                {
                    if (!defaults.ContainsKey(displayFor))
                        defaults.Add(displayFor, new List<string>());

                    defaults[displayFor].Add(FieldDefinition.FieldName);
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