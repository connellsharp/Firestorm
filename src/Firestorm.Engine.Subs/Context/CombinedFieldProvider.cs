using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Fields;

namespace Firestorm.Stems.Fuel.Fields
{
    /// <summary>
    /// A combination of multiple <see cref="IFieldProvider{TItem}"/> used as one.
    /// </summary>
    public class CombinedFieldProvider<TItem> : IFieldProvider<TItem>
        where TItem : class
    {
        private readonly IFieldProvider<TItem>[] _underlyingProviders;

        public CombinedFieldProvider(params IFieldProvider<TItem>[] underlyingProviders)
        {
            _underlyingProviders = underlyingProviders;
        }

        public IEnumerable<string> GetDefaultNames(int nestedBy)
        {
            return _underlyingProviders.SelectMany(provider => provider.GetDefaultNames(nestedBy));
        }

        public IRestResource GetFullResource(string fieldName, IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            return GetThing(fieldName, m => m.GetFullResource(fieldName, item, dataTransaction));
        }

        public IFieldReader<TItem> GetReader(string fieldName)
        {
            return GetThing(fieldName, m => m.GetReader(fieldName));
        }

        public IFieldWriter<TItem> GetWriter(string fieldName)
        {
            return GetThing(fieldName, m => m.GetWriter(fieldName));
        }

        public IFieldDescription GetDescription(string fieldName, CultureInfo cultureInfo)
        {
            return GetThing(fieldName, m => m.GetDescription(fieldName, cultureInfo));
        }

        private TThing GetThing<TThing>(string fieldName, Func<IFieldProvider<TItem>, TThing> getThingFunc)
        {
            foreach (IFieldProvider<TItem> provider in _underlyingProviders)
            {
                if (provider.FieldExists(fieldName))
                    return getThingFunc(provider);
            }

            throw new KeyNotFoundException("Field name '" + fieldName + "' was not found.");
        }

        public bool FieldExists(string fieldName)
        {
            return _underlyingProviders.Any(provider => provider.FieldExists(fieldName));
        }
    }
}