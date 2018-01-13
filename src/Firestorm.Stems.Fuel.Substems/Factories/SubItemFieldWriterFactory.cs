using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Stems.Fuel.Resolving.Factories;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel.Substems.Factories
{
    /// <summary>
    /// Field value factory to create <see cref="SubItemFieldWriter{TItem,TNav}"/>.
    /// </summary>
    internal class SubItemFieldWriterFactory<TItem, TNav, TSubstem> : IFactory<IFieldWriter<TItem>,TItem>
        where TItem : class
        where TNav : class, new()
        where TSubstem : Stem<TNav>
    {
        private readonly Expression<Func<TItem, TNav>> _navigationExpression;

        [UsedImplicitly]
        public SubItemFieldWriterFactory(Expression<Func<TItem, TNav>> navigationExpression)
        {
            _navigationExpression = navigationExpression;
        }

        public IFieldWriter<TItem> Get(Stem<TItem> stem)
        {
            StemsEngineSubContext<TNav> subContext = SubstemEngineSubContextCreator<TItem, TNav, TSubstem>.StemEngineContextFields(stem);
            return new SubItemFieldWriter<TItem, TNav>(_navigationExpression, subContext, null);
        }
    }
}