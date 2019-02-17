using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Stems.Fuel;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using JetBrains.Annotations;

namespace Firestorm.Stems.Essentials.Factories.Factories
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
            var substemCreator = new SubstemEngineSubContextCreator<TItem, TNav, TSubstem>(stem);
            StemsEngineSubContext<TNav> subContext = substemCreator.GetEngineContext();

            return new SubItemFieldReader<TItem, TNav>(_navigationExpression, subContext);
        }
    }
}