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
    internal class SubCollectionFieldWriterFactory<TItem, TProperty, TNav, TSubstem> : IFactory<IFieldWriter<TItem>,TItem>
        where TItem : class
        where TNav : class, new()
        where TSubstem : Stem<TNav>
        where TProperty : IEnumerable<TNav>
    {
        private readonly Expression<Func<TItem, TProperty>> _navigationExpression;

        [UsedImplicitly]
        public SubCollectionFieldWriterFactory(Expression<Func<TItem, TProperty>> navigationExpression)
        {
            _navigationExpression = navigationExpression;
        }

        public IFieldWriter<TItem> Get(Stem<TItem> stem)
        {
            StemsEngineSubContext<TNav> subContext = SubstemEngineSubContextCreator<TItem, TNav, TSubstem>.StemEngineContextFields(stem);
            return new SubCollectionFieldWriter<TItem, TProperty, TNav>(_navigationExpression, subContext, null);
        }
    }
}