using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Engine.Identifiers;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Identifiers
{
    /// <summary>
    /// Builds identifier info from <see cref="IdentifierAttribute"/>s placed on properties and predicate methods in a Stem.
    /// </summary>
    public class MultiExpressionIdentifierInfo<TItem, TReturn> : IIdentifierInfo<TItem>
        where TItem : class
    {
        private readonly Expression<Func<TItem, IEnumerable<TReturn>>> _getterExpr;
        private readonly Func<string, Expression<Func<TItem, bool>>> _predicateGetter;

        public MultiExpressionIdentifierInfo([NotNull] Expression<Func<TItem, IEnumerable<TReturn>>> getterExpr)
        {
            _getterExpr = getterExpr ?? throw new ArgumentNullException(nameof(getterExpr));
            _predicateGetter = identifier => IdentifierExpressionHelpers.GetAnyIdentifierPredicate(_getterExpr, identifier);
        }

        public Type IdentifierType
        {
            get { return typeof(TReturn); }
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
                return _predicateGetter(identifier);
            }
            catch (Exception ex) when (ex is FormatException || ex is InvalidCastException)
            {
                // TODO hiding exception.. includes FormatException thrown in ConversionUtility
                return null;
            }
        }

        public bool AllowsUpsert { get; } = false;

        public void SetValue(TItem item, string identifier)
        {
            throw new NotSupportedException("Upsert by setting the identifier is not supported in this collection.");
        }

        public TItem GetAlreadyLoadedItem(string identifier)
        {
            return null;
        }
    }
}