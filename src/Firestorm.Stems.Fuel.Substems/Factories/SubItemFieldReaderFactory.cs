using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Fuel.Resolving.Factories;
using Firestorm.Stems.Fuel.Substems.Handlers;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel.Substems.Factories
{
    /// <summary>
    /// Field value factory to create <see cref="SubItemFieldReader{TItem,TNav}"/>.
    /// </summary>
    internal class SubItemFieldReaderFactory<TItem, TNav, TSubstem> : IFactory<IFieldReader<TItem>,TItem>
        where TItem : class
        where TNav : class
        where TSubstem : Stem<TNav>
    {
        private readonly Expression<Func<TItem, TNav>> _navigationExpression;

        [UsedImplicitly]
        public SubItemFieldReaderFactory(Expression<Func<TItem, TNav>> navigationExpression)
        {
            _navigationExpression = navigationExpression;
        }

        public IFieldReader<TItem> Get(Stem<TItem> stem)
        {
            StemsEngineSubContext<TNav> subContext = SubstemEngineSubContextCreator<TItem, TNav, TSubstem>.StemEngineContextFields(stem);
            return new SubItemFieldReader<TItem, TNav>(_navigationExpression, subContext);
        }
    }
}