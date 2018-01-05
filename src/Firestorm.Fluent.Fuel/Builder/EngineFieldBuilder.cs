using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Additives.Writers;
using Firestorm.Engine.Identifiers;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Fluent.Fuel.Engine;
using Firestorm.Fluent.Fuel.Models;

namespace Firestorm.Fluent.Fuel.Builder
{
    internal class EngineFieldBuilder<TItem, TField> : IApiFieldBuilder<TItem, TField>
        where TItem : class
    {
        private readonly ApiFieldModel<TItem> _fieldModel;
        private Expression<Func<TItem, TField>> _expression;

        internal EngineFieldBuilder(ApiFieldModel<TItem> fieldModel)
        {
            _fieldModel = fieldModel;
        }

        public void AddExpression(Expression<Func<TItem, TField>> expression)
        {
            Debug.Assert(_expression == null);

            _expression = expression;
            _fieldModel.Reader = new ExpressionFieldReader<TItem, TField>(_expression);
        }

        public IApiFieldBuilder<TItem, TField> HasName(string fieldName)
        {
            _fieldModel.Name = fieldName;
            return this;
        }

        public IApiFieldBuilder<TItem, TField> AllowWrite()
        {
            _fieldModel.Writer = new PropertyExpressionFieldWriter<TItem, TField>(_expression);
            return this;
        }

        public IApiItemBuilder<TNavItem> IsItem<TNavItem>()
            where TNavItem : class, TField, new()
        {
            var itemModel = new ApiItemModel<TNavItem>(null); // can be null because GetRootCollection never called for navigation properties
            var castedExpression = _expression as Expression<Func<TItem, TNavItem>>; // should cast fine due to generic constraint
            IEngineSubContext<TNavItem> subContext = GetSubContext(itemModel);

            _fieldModel.Reader = new SubItemFieldReader<TItem, TNavItem>(castedExpression, subContext);
            _fieldModel.FieldResourceGetter = new SubItemResourceGetter<TItem, TNavItem>(castedExpression, subContext);
            _fieldModel.Writer = new SubItemFieldWriter<TItem, TNavItem>(castedExpression, subContext);

            var itemBuilder = new EngineItemBuilder<TNavItem>(itemModel);
            return itemBuilder;
        }

        public IApiItemBuilder<TNavItem> IsCollection<TCollection, TNavItem>()
            where TCollection : TField, IEnumerable<TNavItem>
            where TNavItem : class, new()
        {
            var itemModel = new ApiItemModel<TNavItem>(null); // can be null because GetRootCollection never called for navigation properties
            var castedExpression = _expression as Expression<Func<TItem, TCollection>>; // should cast fine due to generic constraint
            IEngineSubContext<TNavItem> subContext = GetSubContext(itemModel);

            _fieldModel.Reader = new SubCollectionFieldReader<TItem, TCollection, TNavItem>(castedExpression, subContext);
            _fieldModel.FieldResourceGetter = new SubCollectionResourceGetter<TItem, TCollection, TNavItem>(castedExpression, subContext);
            _fieldModel.Writer = new SubCollectionFieldWriter<TItem, TCollection, TNavItem>(castedExpression, subContext);

            var itemBuilder = new EngineItemBuilder<TNavItem>(itemModel);
            return itemBuilder;
        }

        private static FluentEngineSubContext<TNavItem> GetSubContext<TNavItem>(ApiItemModel<TNavItem> itemModel)
            where TNavItem : class, new()
        {
            return new FluentEngineSubContext<TNavItem>(itemModel.Fields.ToDictionary(f => f.Name), itemModel.Identifiers.ToDictionary(i => i.Name));
        }
    }
}