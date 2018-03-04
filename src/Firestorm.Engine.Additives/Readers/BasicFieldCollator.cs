using System;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Readers
{
    /// <summary>
    /// A collator that enables basic filtering and sorting using the select expression
    /// from an <see cref="IFieldReader{TItem}"/> object.
    /// </summary>
    public class BasicFieldCollator<TItem> : IFieldCollator<TItem>
        where TItem : class
    {
        private readonly IFieldReader<TItem> _reader;

        public BasicFieldCollator(IFieldReader<TItem> reader)
        {
            _reader = reader;
        }
        
        public Expression GetFilterExpression(ParameterExpression itemPram, FilterComparisonOperator comparisonOperator, string valueString)
        {
            var getterMemberExpr = _reader.GetSelectExpression(itemPram);
            if (getterMemberExpr == null)
                throw new InvalidCastException("Getter expression returned from IFieldReader is not of type MemberExpression.");

            object valueObj = ConversionUtility.ConvertString(valueString, _reader.FieldType);
            ConstantExpression variableExpression = Expression.Constant(valueObj, _reader.FieldType); // TODO variable not constant?

            var operatorMethod = GetOperatorExpressionMethod(comparisonOperator);
            Expression predicateExpression = operatorMethod.Invoke(getterMemberExpr, variableExpression);
            return predicateExpression;
        }

        public LambdaExpression GetSortExpression(ParameterExpression itemPram)
        {
            Expression getterExpr = _reader.GetSelectExpression(itemPram);
            return Expression.Lambda(getterExpr, itemPram);
        }

        [Pure]
        private static Func<Expression, ConstantExpression, Expression> GetOperatorExpressionMethod(FilterComparisonOperator comparisonOperator)
        {
            switch (comparisonOperator)
            {
                case FilterComparisonOperator.Equals:
                    return Expression.Equal;

                case FilterComparisonOperator.NotEquals:
                    return Expression.NotEqual;

                case FilterComparisonOperator.GreaterThan:
                    return Expression.GreaterThan;

                case FilterComparisonOperator.GreaterThanOrEquals:
                    return Expression.GreaterThanOrEqual;

                case FilterComparisonOperator.LessThan:
                    return Expression.LessThan;

                case FilterComparisonOperator.LessThanOrEquals:
                    return Expression.LessThanOrEqual;

                case FilterComparisonOperator.Contains:
                    return ExpressionCallComparisonMethod("Contains");

                case FilterComparisonOperator.StartsWith:
                    return ExpressionCallComparisonMethod("StartsWith");

                case FilterComparisonOperator.EndsWith:
                    return ExpressionCallComparisonMethod("EndsWith");

                case FilterComparisonOperator.IsIn:
                    return ExpressionCallComparisonMethod("Contains", true);
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(comparisonOperator), comparisonOperator, null);
            }
        }

        private static Func<Expression, ConstantExpression, Expression> ExpressionCallComparisonMethod(string methodName, bool reverse = false)
        {
            MethodInfo method = typeof(string).GetMethod(methodName, new[] { typeof(string) });

            if (reverse)
                return (getter, constant) => Expression.Call(constant, method, getter);
            else
                return (getter, constant) => Expression.Call(getter, method, constant);
        }
    }
}