using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Additives.Writers;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Engine.Subs.Repositories;
using Firestorm.Fluent.Fuel.Engine;
using Firestorm.Fluent.Fuel.Models;

namespace Firestorm.Fluent.Fuel.Builder
{
    internal class EngineFieldBuilder<TItem, TField> : IApiFieldBuilder<TItem, TField>
        where TItem : class
    {
        private readonly ApiFieldModel<TItem> _fieldModel;
        private readonly Expression<Func<TItem, TField>> _expression;
        private INavigationSetter<TItem, TField> _setter;

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

        public IApiFieldBuilder<TItem, TField> AllowCollate()
        {
            _fieldModel.Collator = new BasicFieldCollator<TItem>(_fieldModel.Reader);
            return this;
        }

        public IApiFieldBuilder<TItem, TField> AllowWrite()
        {
            _setter = new DefaultNavigationSetter<TItem, TField>(_expression);
            _fieldModel.Writer = new PropertyExpressionFieldWriter<TItem, TField>(_expression);
            return this;
        }

        public IApiFieldBuilder<TItem, TField> AllowWrite(Action<TItem, TField> action)
        {
            _setter = new ActionNavigationSetter<TItem, TField>(action);
            _fieldModel.Writer = new NavigationSetterFieldWriter<TItem, TField>(_setter);
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
            IEngineSubContext<TNavItem> subContext = new FluentEngineSubContext<TNavItem>(itemModel);

            var castedExpression = _expression as Expression<Func<TItem, TNavItem>>; // should cast fine due to generic constraint

            var navTools = new SubWriterTools<TItem, TNavItem, TNavItem>(castedExpression, itemModel.Events, _setter);

            _fieldModel.Reader = new SubItemFieldReader<TItem, TNavItem>(castedExpression, subContext);
            _fieldModel.Collator = new NotSupportedFieldCollator<TItem>("sub items");
            _fieldModel.ResourceGetter = new SubItemResourceGetter<TItem, TNavItem>(castedExpression, subContext);
            _fieldModel.Writer = new SubItemFieldWriter<TItem, TNavItem>(navTools, subContext);

            var itemBuilder = new EngineItemBuilder<TNavItem>(itemModel);
            return itemBuilder;
        }

        public IApiItemBuilder<TNavItem> IsCollection<TCollection, TNavItem>()
            where TCollection : class, TField, IEnumerable<TNavItem>
            where TNavItem : class, new()
        {
            var itemModel = new ApiItemModel<TNavItem>(null); // can be null because GetRootCollection never called for navigation properties
            IEngineSubContext<TNavItem> subContext = new FluentEngineSubContext<TNavItem>(itemModel);

            var castedExpression = _expression as Expression<Func<TItem, TCollection>>; // should cast fine due to generic constraint
            var castedSetter = _setter as INavigationSetter<TItem, TCollection>;

            var navTools = new SubWriterTools<TItem, TCollection, TNavItem>(castedExpression, itemModel.Events, castedSetter);

            _fieldModel.Reader = new SubCollectionFieldReader<TItem, TCollection, TNavItem>(castedExpression, subContext);
            _fieldModel.Collator = new NotSupportedFieldCollator<TItem>("sub collections");
            _fieldModel.ResourceGetter = new SubCollectionResourceGetter<TItem, TCollection, TNavItem>(navTools, subContext);
            _fieldModel.Writer = new SubCollectionFieldWriter<TItem, TCollection, TNavItem>(navTools, subContext);

            var itemBuilder = new EngineItemBuilder<TNavItem>(itemModel);
            return itemBuilder;
        }
    }
}