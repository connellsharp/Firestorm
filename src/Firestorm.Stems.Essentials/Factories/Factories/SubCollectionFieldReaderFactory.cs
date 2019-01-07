using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Stems.Fuel;
using Firestorm.Stems.Fuel.Resolving.Factories;
using JetBrains.Annotations;

namespace Firestorm.Stems.Essentials.Factories.Factories
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
            var substemCreator = new SubstemEngineSubContextCreator<TItem, TNav, TSubstem>(stem);
            StemsEngineSubContext<TNav> subContext = substemCreator.GetEngineContext();

            return new SubCollectionFieldReader<TItem, TCollection, TNav>(_navigationExpression, subContext);
        }
    }
}