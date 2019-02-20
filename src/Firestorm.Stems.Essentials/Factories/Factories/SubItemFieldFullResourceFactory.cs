using System;
using System.Linq.Expressions;
using Firestorm.Engine.Subs;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Fuel;
using Firestorm.Stems.Fuel.Analysis;
using JetBrains.Annotations;

namespace Firestorm.Stems.Essentials.Factories.Factories
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
        private readonly FieldDefinition _definition;

        [UsedImplicitly]
        public SubItemFieldFullResourceFactory(Expression<Func<TItem, TNav>> navigationExpression, FieldDefinition definition)
        {
            _navigationExpression = navigationExpression;
            _definition = definition;
        }

        public IFieldResourceGetter<TItem> Get(Stem<TItem> stem)
        {
            var substemCreator = new SubstemEngineSubContextCreator<TItem, TNav, TSubstem>(stem);
            StemDataChangeEvents<TNav> stemEvents = substemCreator.GetDataChangeEvents();
            StemsEngineSubContext<TNav> subContext = substemCreator.GetEngineContext();

            MethodSetter<TItem, TNav> setter = MethodSetter<TItem, TNav>.FromDefinition(_definition, stem);
            var tools = new SubWriterTools<TItem, TNav, TNav>(_navigationExpression, stemEvents, setter);

            return new SubItemResourceGetter<TItem, TNav>(tools, subContext);
        }
    }
}