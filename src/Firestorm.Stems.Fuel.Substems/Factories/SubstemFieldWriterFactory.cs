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
    /// Field value factory to create <see cref="SubCollectionFieldWriter{TItem,TNav}"/> and <see cref="SubItemFieldWriter{TItem,TNav}"/>.
    /// </summary>
    internal class SubstemFieldWriterFactory<TItem, TNav, TSubstem> : IFactory<IFieldWriter<TItem>, TItem>
        where TItem : class
        where TNav : class, new()
        where TSubstem : Stem<TNav>
    {
        private readonly Func<StemEngineSubContext<TNav>, IFieldWriter<TItem>> _createFieldValue;

        [UsedImplicitly]
        public SubstemFieldWriterFactory(Expression<Func<TItem, IEnumerable<TNav>>> navigationExpression)
        {
            _createFieldValue = f => new SubCollectionFieldWriter<TItem, TNav>(navigationExpression, f);
        }

        [UsedImplicitly]
        public SubstemFieldWriterFactory(Expression<Func<TItem, TNav>> navigationExpression)
        {
            _createFieldValue = f => new SubItemFieldWriter<TItem, TNav>(navigationExpression, f);
        }

        public IFieldWriter<TItem> Get(Stem<TItem> stem)
        {
            StemEngineSubContext<TNav> subContext = SubstemUtilities.StemEngineContextFields<TItem, TNav, TSubstem>(stem);
            return _createFieldValue(subContext);
        }

    }
}