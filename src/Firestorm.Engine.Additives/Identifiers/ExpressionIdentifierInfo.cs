using System;
using System.Linq.Expressions;
using Firestorm.Engine.Identifiers;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Identifiers
{
    /// <summary>
    /// Builds identifier info from <see cref="IdentifierAttribute"/>s placed on properties and predicate methods in a Stem.
    /// </summary>
    public class ExpressionIdentifierInfo<TItem, TReturn> : IIdentifierInfo<TItem>
        where TItem : class
    {
        private readonly Expression<Func<TItem, TReturn>> _getterExpr;

        public ExpressionIdentifierInfo([NotNull] Expression<Func<TItem, TReturn>> getterExpr)
        {
            _getterExpr = getterExpr ?? throw new ArgumentNullException(nameof(getterExpr));
        }

        public Type IdentifierType
        {
            get { return _getterExpr.ReturnType; }
        }

        public object GetValue(TItem item)
        {
            return _getterExpr.Compile().DynamicInvoke(item);
        }

        public Expression GetGetterExpression(ParameterExpression parameterExpr)
        {
            // TODO does this keep swapping the same parameter?
            var replacer = new ParameterReplacerVisitor(_getterExpr.Parameters[0], parameterExpr);
            return replacer.Visit(_getterExpr.Body);
        }

        public Expression<Func<TItem, bool>> GetPredicate(string identifier)
        {
            try
            {
                return IdentifierExpressionHelpers.GetIdentifierPredicate(_getterExpr, identifier);
            }
            catch (Exception ex) when (ex is FormatException || ex is InvalidCastException)
            {
                // TODO hiding exception.. includes FormatException thrown in ConversionUtility
                return null;
            }
        }

        public bool AllowsUpsert { get; } = true; // TODO really always true?

        public void SetValue(TItem item, string identifier)
        {
            var propertyInfo = PropertyInfoUtilities.GetPropertyInfoFromLambda(_getterExpr);
            object convertedValue = ConversionUtility.ConvertValue(identifier, propertyInfo.PropertyType);
            propertyInfo.SetValue(item, convertedValue);

            // TODO use below?
            //IdentifierExpressionHelpers.SetIdentifier(item, _getterExpr, identifier);
        }

        public TItem GetAlreadyLoadedItem(string identifier)
        {
            return null;
        }
    }
}