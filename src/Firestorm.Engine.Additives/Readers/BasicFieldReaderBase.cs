using System;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Readers
{
    /// <summary>
    /// Base class for basic fields that don't need database return values replacing with more complex objects.
    /// </summary>
    public abstract class BasicFieldReaderBase<TItem> : IFieldReader<TItem>
        where TItem : class
    {
        public abstract Type FieldType { get; }

        protected abstract Expression GetGetterExpression(ParameterExpression parameterExpr);

        public IFieldValueReplacer<TItem> Replacer { get; } = null;

        public Expression GetSelectExpression(ParameterExpression itemPram)
        {
            return GetGetterExpression(itemPram);
        }
        
        public Expression GetFilterExpression(ParameterExpression itemPram, FilterComparisonOperator comparisonOperator, string valueString)
        {
            var getterMemberExpr = GetSelectExpression(itemPram);
            if (getterMemberExpr == null)
                throw new InvalidCastException("Getter expression returned from IFieldReader is not of type MemberExpression.");

            object valueObj = ConversionUtility.ConvertString(valueString, FieldType);
            ConstantExpression variableExpression = Expression.Constant(valueObj, FieldType); // TODO variable not constant?

            var operatorMethod = GetOperatorExpressionMethod(comparisonOperator);
            Expression predicateExpression = operatorMethod.Invoke(getterMemberExpr, variableExpression);
            return predicateExpression;
        }

        public LambdaExpression GetSortExpression(ParameterExpression itemPram)
        {
            Expression getterExpr = GetSelectExpression(itemPram);
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
