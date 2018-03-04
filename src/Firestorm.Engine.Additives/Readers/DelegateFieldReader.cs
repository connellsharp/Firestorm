using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Readers
{
    /// <summary>
    /// Uses a separate getter method that takes the item as a parameter to map the field to the method's return value.
    /// These can't be used in the expressions themselves but take advantage of the 'replacers'.
    /// </summary>
    public class DelegateFieldReader<TItem, TValue> : IFieldReader<TItem>
        where TItem : class
    {
        private readonly Func<TItem, TValue> _getterFunc;

        public DelegateFieldReader([NotNull] Func<TItem, TValue> getterFunc)
        {
            _getterFunc = getterFunc
                          ?? throw new ArgumentNullException(nameof(getterFunc));
        }

        public Type FieldType => typeof(TValue);

        public Expression GetSelectExpression(ParameterExpression itemPram)
        {
            throw new NotSupportedException("This field cannot be used as part of an expression.");
        }

        public IFieldValueReplacer<TItem> Replacer { get; } = null;
    }
}