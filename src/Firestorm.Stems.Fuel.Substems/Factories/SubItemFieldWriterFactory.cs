using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Stems.Attributes.Definitions;
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
        private readonly FieldDefinition _definition;

        [UsedImplicitly]
        public SubItemFieldWriterFactory(Expression<Func<TItem, TNav>> navigationExpression, FieldDefinition definition)
        {
            _navigationExpression = navigationExpression;
            _definition = definition;
        }

        public IFieldWriter<TItem> Get(Stem<TItem> stem)
        {
            StemsEngineSubContext<TNav> subContext = SubstemEngineSubContextCreator<TItem, TNav, TSubstem>.StemEngineContextFields(stem);
            MethodSetter<TItem, TNav> setter = MethodSetter<TItem, TNav>.FromDefinition(_definition, stem);
            var tools = new SubWriterTools<TItem, TNav, TNav>(_navigationExpression, null, setter);
            return new SubItemFieldWriter<TItem, TNav>(tools, subContext);
        }
    }
}