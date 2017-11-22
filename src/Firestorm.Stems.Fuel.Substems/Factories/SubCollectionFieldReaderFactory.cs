using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Fuel.Resolving.Factories;
using Firestorm.Stems.Fuel.Substems.Handlers;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel.Substems.Factories
{
    /// <summary>
    /// Field value factory to create <see cref="SubCollectionFieldReader{TItem,TProperty,TNav}"/>.
    /// </summary>
    internal class SubCollectionFieldReaderFactory<TItem, TCollection, TNav, TSubstem> : IFactory<IFieldReader<TItem>,TItem>
        where TItem : class
        where TNav : class, new()
        where TSubstem : Stem<TNav>
        where TCollection : IEnumerable<TNav>
    {
        private readonly Expression<Func<TItem, TCollection>> _navigationExpression;

        [UsedImplicitly]
        public SubCollectionFieldReaderFactory(Expression<Func<TItem, TCollection>> navigationExpression)
        {
            _navigationExpression = navigationExpression;
        }

        public IFieldReader<TItem> Get(Stem<TItem> stem)
        {
            StemsEngineSubContext<TNav> subContext = SubstemEngineSubContextCreator<TItem, TNav, TSubstem>.StemEngineContextFields(stem);
            return new SubCollectionFieldReader<TItem, TCollection, TNav>(_navigationExpression, subContext);
        }
    }
}