using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Fuel;
using Firestorm.Stems.Fuel.Resolving.Factories;
using JetBrains.Annotations;

namespace Firestorm.Stems.Essentials.Factories.Factories
{
    /// <summary>
    /// Field value factory to create <see cref="SubCollectionFieldWriter{TItem,TProperty,TNav}"/>.
    /// </summary>
    internal class SubCollectionFieldWriterFactory<TItem, TCollection, TNav, TSubstem> : IFactory<IFieldWriter<TItem>,TItem>
        where TItem : class
        where TCollection : class, IEnumerable<TNav>
        where TNav : class, new()
        where TSubstem : Stem<TNav>
    {
        private readonly Expression<Func<TItem, TCollection>> _navigationExpression;
        private readonly FieldDefinition _definition;

        [UsedImplicitly]
        public SubCollectionFieldWriterFactory(Expression<Func<TItem, TCollection>> navigationExpression, FieldDefinition definition)
        {
            _navigationExpression = navigationExpression;
            _definition = definition;
        }

        public IFieldWriter<TItem> Get(Stem<TItem> stem)
        {
            var substemCreator = new SubstemEngineSubContextCreator<TItem, TNav, TSubstem>(stem);
            StemDataChangeEvents<TNav> repoEvents = substemCreator.GetDataChangeEvents();
            StemsEngineSubContext<TNav> subContext = substemCreator.GetEngineContext();

            MethodSetter<TItem, TCollection> setter = MethodSetter<TItem, TCollection>.FromDefinition(_definition, stem);
            var tools = new SubWriterTools<TItem, TCollection, TNav>(_navigationExpression, repoEvents, setter);

            return new SubCollectionFieldWriter<TItem, TCollection, TNav>(tools, subContext);
        }
    }
}