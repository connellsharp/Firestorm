using System;
using System.Linq.Expressions;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Stems.Fuel.Fields;
using Firestorm.Stems.Fuel.Resolving.Factories;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel.Substems.Factories
{
    /// <summary>
    /// Field value factory to create <see cref="SubItemFieldFullResource{TItem,TNav}"/>.
    /// </summary>
    internal class SubItemFieldFullResourceFactory<TItem, TNav, TSubstem> : IFactory<IFieldResourceGetter<TItem>, TItem>
        where TItem : class
        where TNav : class, new()
        where TSubstem : Stem<TNav>
    {
        private readonly Expression<Func<TItem, TNav>> _navigationExpression;

        [UsedImplicitly]
        public SubItemFieldFullResourceFactory(Expression<Func<TItem, TNav>> navigationExpression)
        {
            _navigationExpression = navigationExpression;
        }

        public IFieldResourceGetter<TItem> Get(Stem<TItem> stem)
        {
            var substemCreator = new SubstemEngineSubContextCreator<TItem, TNav, TSubstem>(stem);
            StemsEngineSubContext<TNav> subContext = substemCreator.GetEngineContext();

            return new SubItemResourceGetter<TItem, TNav>(_navigationExpression, subContext);
        }
    }
}