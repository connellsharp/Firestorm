using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Firestorm.Engine.Fields
{
    /// <summary>
    /// Creates LINQ <see cref="Expression"/>s for the Firestorm Engine to read a field from an item or collection of items.
    /// Expressions should be reusable for many items in a collection.
    /// </summary>
    public interface IFieldReader<in TItem> : IFieldHandler<TItem>
    {
        /// <summary>
        /// The type of the .NET field when used in an anonymous object.
        /// </summary>
        Type FieldType { get; }

        /// <summary>
        /// The lambda expression body used to select the field from the given item.
        /// </summary>
        Expression GetSelectExpression(ParameterExpression itemPram);

        /// <summary>
        /// Replaces values after they have been loaded into memory.
        /// </summary>
        [CanBeNull]
        IFieldValueReplacer<TItem> Replacer { get; }

        /// <summary>
        /// Gets a predicate expression to filter the collection.
        /// </summary>
        Expression GetFilterExpression(ParameterExpression itemPram, FilterComparisonOperator comparisonOperator, string valueString);

        /// <summary>
        /// Gets a select expression to sort the collection by.
        /// </summary>
        LambdaExpression GetSortExpression(ParameterExpression itemPram);
    }
}