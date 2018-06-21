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
        public DelegateFieldReader([NotNull] Func<TItem, TValue> getterFunc)
        {
            Replacer = new DelegateWholeItemReplacer<TItem, TValue>(getterFunc);
        }

        public Type FieldType => typeof(object); // typeof(TValue);

        public Expression GetSelectExpression(ParameterExpression itemPram)
        {
            return itemPram;
        }

        public IFieldValueReplacer<TItem> Replacer { get; }
    }
}