using System.Linq.Expressions;

namespace Firestorm.Engine.Fields
{
    /// <summary>
    /// Creates LINQ Expressions that allow the Firestorm Engine to filter and sort collections by fields.
    /// </summary>
    public interface IFieldCollator<in TItem> : IFieldHandler<TItem>
    {
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