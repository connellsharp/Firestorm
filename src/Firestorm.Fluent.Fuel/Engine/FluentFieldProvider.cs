using System.Collections.Generic;
using System.Globalization;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Writers;
using Firestorm.Engine.Deferring;
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
            return _fieldModels[fieldName].ResourceGetter?.GetFullResource(item, dataTransaction);
        }

        public IFieldReader<TItem> GetReader(string fieldName)
        {
            return _fieldModels[fieldName].Reader;
        }

        public IFieldCollator<TItem> GetCollator(string fieldName)
        {
            return _fieldModels[fieldName].Collator;
        }

        public IFieldWriter<TItem> GetWriter(string fieldName)
        {
            ApiFieldModel<TItem> model = _fieldModels[fieldName];
            return model.Writer ?? new ConfirmOnlyFieldWriter<TItem>(fieldName, model.Reader);
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