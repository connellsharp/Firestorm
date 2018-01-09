using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Context;
using Firestorm.Fluent.Fuel.Models;

namespace Firestorm.Fluent.Fuel.Engine
{
    internal class FluentFieldProvider<TItem> : ILocatableFieldProvider<TItem>
        where TItem : class
    {
        private readonly IReadOnlyDictionary<string, ApiFieldModel<TItem>> _fieldModels;

        public FluentFieldProvider(IEnumerable<ApiFieldModel<TItem>> fieldModels)
        {
            _fieldModels = new LazyDictionary<ApiFieldModel<TItem>>(fieldModels, f => f.Name);
        }

        public IEnumerable<string> GetDefaultNames(int nestedBy)
        {
            // TODO share Display enum functionality from Stems? And show nestedBy > x ?

            return _fieldModels.Keys;
        }

        public bool FieldExists(string fieldName)
        {
            return _fieldModels.ContainsKey(fieldName);
        }

        public IRestResource GetFullResource(string fieldName, IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            return _fieldModels[fieldName].ResourceGetter.GetFullResource(item, dataTransaction);
        }

        public IFieldReader<TItem> GetReader(string fieldName)
        {
            return _fieldModels[fieldName].Reader;
        }

        public IFieldWriter<TItem> GetWriter(string fieldName)
        {
            return _fieldModels[fieldName].Writer;
        }

        public IFieldDescription GetDescription(string fieldName, CultureInfo cultureInfo)
        {
            return _fieldModels[fieldName].Description;
        }

        public IItemLocator<TItem> GetLocator(string fieldName)
        {
            return _fieldModels[fieldName].Locator;
        }
    }
}