using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Stems.Fuel.Fields;
using Firestorm.Stems.Fuel.Resolving;
using Firestorm.Stems.Fuel.Resolving.Factories;
using Firestorm.Stems.Fuel.Substems.Handlers;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel.Substems.Factories
{
    /// <summary>
    /// Field value factory to create <see cref="SubCollectionFieldReader{TItem,TNav}"/> and <see cref="SubItemFieldReader{TItem,TNav}"/>.
    /// </summary>
    internal class SubstemFieldFullResourceFactory<TItem, TNav, TSubstem> : IFactory<IFieldResourceGetter<TItem>,TItem>
        where TItem : class
        where TNav : class, new()
        where TSubstem : Stem<TNav>
    {
        private readonly Func<StemEngineSubContext<TNav>, IFieldResourceGetter<TItem>>  _createFieldValue;

        [UsedImplicitly]
        public SubstemFieldFullResourceFactory(Expression<Func<TItem, IEnumerable<TNav>>> navigationExpression)
        {
            _createFieldValue = f => new SubCollectionResourceGetter<TItem, TNav>(navigationExpression, f);
        }

        [UsedImplicitly]
        public SubstemFieldFullResourceFactory(Expression<Func<TItem, TNav>> navigationExpression)
        {
            _createFieldValue = f => new SubItemResourceGetter<TItem, TNav>(navigationExpression, f);
        }

        public IFieldResourceGetter<TItem> Get(Stem<TItem> stem)
        {
            StemEngineSubContext<TNav> subContext = StemEngineContextFields(stem);
            return _createFieldValue(subContext);
        }

        private static StemEngineSubContext<TNav> StemEngineContextFields(Stem<TItem> stem)
        {
            var autoActivator = new AutoActivator(stem.Configuration.DependencyResolver);
            var substem = autoActivator.CreateInstance<TSubstem>();
            substem.SetParent(stem);

            return new StemEngineSubContext<TNav>(substem);
        }
    }
}