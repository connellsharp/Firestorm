using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Stems.Fuel.Resolving.Factories;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel.Substems.Factories
{
    /// <summary>
    /// Field value factory to create <see cref="SubCollectionFieldWriter{TItem,TProperty,TNav}"/>.
    /// </summary>
    internal class SubCollectionFieldWriterFactory<TItem, TCollection, TNav, TSubstem> : IFactory<IFieldWriter<TItem>,TItem>
        where TItem : class
        where TCollection : class, IEnumerable<TNav>
        where TNav : class, new()
        where TSubstem : Stem<TNav>
    {
        private readonly Expression<Func<TItem, TCollection>> _navigationExpression;

        [UsedImplicitly]
        public SubCollectionFieldWriterFactory(Expression<Func<TItem, TCollection>> navigationExpression)
        {
            _navigationExpression = navigationExpression;
        }

        public IFieldWriter<TItem> Get(Stem<TItem> stem)
        {
            StemsEngineSubContext<TNav> subContext = SubstemEngineSubContextCreator<TItem, TNav, TSubstem>.StemEngineContextFields(stem);
            var tools = new SubWriterTools<TItem, TCollection, TNav>(_navigationExpression, null, null);
            return new SubCollectionFieldWriter<TItem, TCollection, TNav>(tools, subContext);
        }
    }
}