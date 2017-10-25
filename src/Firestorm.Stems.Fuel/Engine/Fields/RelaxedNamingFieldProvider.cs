using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Firestorm.Engine;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Naming;

namespace Firestorm.Stems.Fuel.Fields
{
    /// <summary>
    /// Field provider wrapper that uses casing rules to allowed multiple conventions for the same fields.
    /// </summary>
    internal class RelaxedNamingFieldProvider<TItem> : IStemFieldProvider<TItem>
        where TItem : class
    {
        private readonly IStemFieldProvider<TItem> _underlyingFieldProvider;
        private readonly NamingConventionSwitcher _conventionSwitcher;

        internal RelaxedNamingFieldProvider(IStemFieldProvider<TItem> underlyingFieldProvider, NamingConventionSwitcher conventionSwitcher)
        {
            _underlyingFieldProvider = underlyingFieldProvider;
            _conventionSwitcher = conventionSwitcher;
        }

        public IEnumerable<string> GetDefaultNames(int nestedBy)
        {
            return _underlyingFieldProvider.GetDefaultNames(nestedBy).Select(_conventionSwitcher.ConvertCodedToDefault);
        }

        public IRestResource GetFullResource(string fieldName, IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            string switchedFieldName = _conventionSwitcher.ConvertRequestedToCoded(fieldName);
            return _underlyingFieldProvider.GetFullResource(switchedFieldName ?? fieldName, item, dataTransaction);
        }

        public IFieldReader<TItem> GetReader(string fieldName)
        {
            string switchedFieldName = _conventionSwitcher.ConvertRequestedToCoded(fieldName);
            return _underlyingFieldProvider.GetReader(switchedFieldName ?? fieldName);
        }

        public IFieldWriter<TItem> GetWriter(string fieldName)
        {
            string switchedFieldName = _conventionSwitcher.ConvertRequestedToCoded(fieldName);
            return _underlyingFieldProvider.GetWriter(switchedFieldName ?? fieldName);
        }

        public IItemLocator<TItem> GetLocator(string fieldName)
        {
            string switchedFieldName = _conventionSwitcher.ConvertRequestedToCoded(fieldName);
            return _underlyingFieldProvider.GetLocator(switchedFieldName ?? fieldName);
        }

        public IFieldDescription GetDescription(string fieldName, CultureInfo cultureInfo)
        {
            string switchedFieldName = _conventionSwitcher.ConvertRequestedToCoded(fieldName);
            return _underlyingFieldProvider.GetDescription(switchedFieldName ?? fieldName, cultureInfo);
        }

        public bool FieldExists(string fieldName)
        {
            string switchedFieldName = _conventionSwitcher.ConvertRequestedToCoded(fieldName);
            return _underlyingFieldProvider.FieldExists(switchedFieldName ?? fieldName);
        }
    }
}