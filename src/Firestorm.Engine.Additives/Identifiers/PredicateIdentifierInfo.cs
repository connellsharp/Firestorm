using System;
using System.Linq.Expressions;
using Firestorm.Engine.Identifiers;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Identifiers
{
    /// <summary>
    /// Builds identifier info from predicate methods.
    /// </summary>
    public class PredicateIdentifierInfo<TItem> : IIdentifierInfo<TItem>
        where TItem : class
    {
        private readonly Func<string, Expression<Func<TItem, bool>>> _getPredicateFunc;
        private readonly Action<TItem, string> _setter;

        public PredicateIdentifierInfo(Func<string, Expression<Func<TItem, bool>>> getPredicateFunc, [CanBeNull] Action<TItem, string> setter)
        {
            _getPredicateFunc = getPredicateFunc;
            _setter = setter;
        }

        public Type IdentifierType
        {
            get { return typeof(string); }
        }

        public object GetValue(TItem item)
        {
            throw new NotImplementedException("Cannot get value of an identifier from a predicate getter.");
        }

        public Expression GetGetterExpression(ParameterExpression parameterExpr)
        {
            throw new NotImplementedException("Cannot get value of an identifier from a predicate getter.");
        }

        public Expression<Func<TItem, bool>> GetPredicate(string identifier)
        {
            return _getPredicateFunc(identifier);
        }

        public void SetValue(TItem item, string identifier)
        {
            if (_setter == null)
                throw new NotSupportedException("Upsert by setting the identifier is not supported in this collection.");

            _setter.Invoke(item, identifier);
        }

        public bool AllowsUpsert
        {
            get { return _setter != null; }
        }

        public TItem GetAlreadyLoadedItem(string identifier)
        {
            return null;
        }
    }
}