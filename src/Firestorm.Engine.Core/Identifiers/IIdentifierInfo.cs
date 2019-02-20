using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine.Identifiers
{
    /// <summary>
    /// Describes how to get, set and search an item within a collection using an identifier string given in an API request, usually in a URL.
    /// </summary>
    public interface IIdentifierInfo<TItem> : IFieldHandler<TItem> // TODO is it really a *field* handler?
    {
        Type IdentifierType { get; }

        Expression GetGetterExpression(ParameterExpression parameterExpr); // TODO: type safe?

        Expression<Func<TItem, bool>> GetPredicate(string identifier);

        [CanBeNull]
        TItem GetAlreadyLoadedItem(string identifier);

        object GetValue(TItem item);

        void SetValue(TItem item, string identifier);

        bool AllowsUpsert { get; }
    }
}