using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Engine.Subs;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Fuel.Resolving.Factories;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel.Substems.Factories
{
    /// <summary>
    /// Field value factory to create <see cref="SubCollectionFieldFullResource{TItem, TProperty,TNav}"/>.
    /// </summary>
    internal class SubCollectionFieldFullResourceFactory<TItem, TCollection, TNav, TSubstem> : IFactory<IFieldResourceGetter<TItem>, TItem>
        where TItem : class
        where TCollection : class, IEnumerable<TNav>
        where TNav : class, new()
        where TSubstem : Stem<TNav>
    {
        private readonly Expression<Func<TItem, TCollection>> _navigationExpression;
        private readonly FieldDefinition _definition;

        [UsedImplicitly]
        public SubCollectionFieldFullResourceFactory(Expression<Func<TItem, TCollection>> navigationExpression, FieldDefinition definition)
        {
            _navigationExpression = navigationExpression;
            _definition = definition;
        }

        public IFieldResourceGetter<TItem> Get(Stem<TItem> stem)
        {
            var substemCreator = new SubstemEngineSubContextCreator<TItem, TNav, TSubstem>(stem);
            StemDataChangeEvents<TNav> stemEvents = substemCreator.GetRepositoryEvents();
            StemsEngineSubContext<TNav> subContext = substemCreator.GetEngineContext();

            MethodSetter<TItem, TCollection> setter = MethodSetter<TItem, TCollection>.FromDefinition(_definition, stem);
            var tools = new SubWriterTools<TItem, TCollection, TNav>(_navigationExpression, stemEvents, setter);

            return new SubCollectionResourceGetter<TItem, TCollection,TNav>(tools, subContext);
        }
    }
}