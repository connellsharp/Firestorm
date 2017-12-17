using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Engine.Identifiers;

namespace Firestorm.Engine.Additives.Identifiers
{
    /// <summary>
    /// Combines several <see cref="IIdentifierInfo{TItem}"/> into one.
    /// Used when no identifier name is given.
    /// </summary>
    public class CombinedIdentifierInfo<TItem> : IIdentifierInfo<TItem>
        where TItem : class
    {
        private readonly IEnumerable<IIdentifierInfo<TItem>> _infos;

        public CombinedIdentifierInfo(IEnumerable<IIdentifierInfo<TItem>> infos)
        {
            Debug.Assert(infos.Any(), "There are no item references set up."); // TODO proper setup error?

            _infos = infos;
        }

        public object GetValue(TItem item)
        {
            return _infos.First().GetValue(item);
        }

        public Type IdentifierType
        {
            get { return typeof(string); }
        }

        public Expression GetGetterExpression(ParameterExpression parameterExpr)
        {
            return _infos.First().GetGetterExpression(parameterExpr);
        }

        public Expression<Func<TItem, bool>> GetPredicate(string identifier)
        {
            return PredicateUtility.CombinePredicates(_infos.Select(i => i.GetPredicate(identifier)));
        }

        public void SetValue(TItem item, string identifier)
        {
            _infos.First().SetValue(item, identifier);
        }

        public bool AllowsUpsert { get; }

        public TItem GetAlreadyLoadedItem(string identifier)
        {
            foreach (IIdentifierInfo<TItem> identifierInfo in _infos)
            {
                var alreadyLoadedItem = identifierInfo.GetAlreadyLoadedItem(identifier);
                if (alreadyLoadedItem != null)
                    return alreadyLoadedItem;
            }

            return null;
        }
    }
}