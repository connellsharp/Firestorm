using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Additives.Writers;
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
        private readonly Expression<Func<TItem, TField>> _expression;

        internal EngineFieldBuilder(ApiFieldModel<TItem> fieldModel, Expression<Func<TItem, TField>> expression)
        {
            _fieldModel = fieldModel;
            _expression = expression;
        }

        public IApiFieldBuilder<TItem, TField> HasName(string fieldName)
        {
            _fieldModel.Name = fieldName;
            return this;
        }

        public IApiFieldBuilder<TItem, TField> AllowRead()
        {
            _fieldModel.Reader = new ExpressionFieldReader<TItem, TField>(_expression);
            return this;
        }

        public IApiFieldBuilder<TItem, TField> AllowWrite()
        {
            _fieldModel.Writer = new PropertyExpressionFieldWriter<TItem, TField>(_expression);
            return this;
        }

        public IApiFieldBuilder<TItem, TField> AllowLocate()
        {
            _fieldModel.Locator = new IdentifierExpressionItemLocator<TItem, TField>(_expression);
            return this;
        }

        public IApiItemBuilder<TNavItem> IsItem<TNavItem>()
            where TNavItem : class, TField, new()
        {
            var itemModel = new ApiItemModel<TNavItem>(null); // can be null because GetRootCollection never called for navigation properties
            var castedExpression = _expression as Expression<Func<TItem, TNavItem>>; // should cast fine due to generic constraint
            IEngineSubContext<TNavItem> subContext = new FluentEngineSubContext<TNavItem>(itemModel);

            _fieldModel.Reader = new SubItemFieldReader<TItem, TNavItem>(castedExpression, subContext);
            _fieldModel.ResourceGetter = new SubItemResourceGetter<TItem, TNavItem>(castedExpression, subContext); // TODO events
            _fieldModel.Writer = new SubItemFieldWriter<TItem, TNavItem>(castedExpression, subContext, itemModel.Events);

            var itemBuilder = new EngineItemBuilder<TNavItem>(itemModel);
            return itemBuilder;
        }

        public IApiItemBuilder<TNavItem> IsCollection<TCollection, TNavItem>()
            where TCollection : TField, IEnumerable<TNavItem>
            where TNavItem : class, new()
        {
            var itemModel = new ApiItemModel<TNavItem>(null); // can be null because GetRootCollection never called for navigation properties
            var castedExpression = _expression as Expression<Func<TItem, TCollection>>; // should cast fine due to generic constraint
            IEngineSubContext<TNavItem> subContext = new FluentEngineSubContext<TNavItem>(itemModel);

            _fieldModel.Reader = new SubCollectionFieldReader<TItem, TCollection, TNavItem>(castedExpression, subContext);
            _fieldModel.ResourceGetter = new SubCollectionResourceGetter<TItem, TCollection, TNavItem>(castedExpression, subContext, itemModel.Events);
            _fieldModel.Writer = new SubCollectionFieldWriter<TItem, TCollection, TNavItem>(castedExpression, subContext, itemModel.Events);

            var itemBuilder = new EngineItemBuilder<TNavItem>(itemModel);
            return itemBuilder;
        }
    }
}