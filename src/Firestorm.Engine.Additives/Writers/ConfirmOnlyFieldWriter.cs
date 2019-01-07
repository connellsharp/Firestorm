using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Writers
{
    /// <summary>
    /// An <see cref="IFieldWriter{TItem}"/> designed only to confirm if the value has not changed and throw an error if not.
    /// Not actually capable of changing the value.
    /// </summary>
    public class ConfirmOnlyFieldWriter<TItem> : IFieldWriter<TItem>
        where TItem : class
    {
        private readonly string _fieldName;
        private readonly IFieldReader<TItem> _fieldReader;

        public ConfirmOnlyFieldWriter([NotNull] string fieldName, [NotNull] IFieldReader<TItem> fieldReader)
        {
            _fieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            _fieldReader = fieldReader ?? throw new ArgumentNullException(nameof(fieldReader));
        }

        public async Task SetValueAsync(IDeferredItem<TItem> item, object deserializedValue, IDataTransaction dataTransaction)
        {
            object loadedValue;

            try
            {
                ForEachAsyncDelegate<object> forEachAsync = ItemQueryHelper.DefaultForEachAsync; // TODO get from item?
                loadedValue = await ScalarFieldHelper.LoadScalarValueAsync(_fieldReader, item.Query, forEachAsync);
            }
            catch (Exception ex)
            {
                throw new ConfirmOnlyValueLoadException(ex, deserializedValue);
            }

            // TODO find a more efficient way. Possibly generic values too?

            if (loadedValue.Equals(deserializedValue))
                return;

            if (_fieldReader.FieldType.IsEnum)
                loadedValue = Convert.ToInt64(loadedValue);

            if (loadedValue.ToString() == deserializedValue.ToString())
                return;

            throw new InvalidOperationException("Cannot set the '" + _fieldName + "' field. Given value must be equal to the current value.");

        }

        private class ConfirmOnlyValueLoadException : RestApiException
        {
            public ConfirmOnlyValueLoadException(Exception exception, object deserializedValue)
                : base(ErrorStatus.InternalServerError, "Error loading value to confirm whether it is correct. Value: " + deserializedValue, exception)
            { }
        }
    }
}