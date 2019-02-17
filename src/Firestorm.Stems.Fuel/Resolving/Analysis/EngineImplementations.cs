using System;
using System.Collections.Generic;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using Firestorm.Engine.Subs.Context;
using Firestorm.Stems.Fuel.Resolving.Factories;

namespace Firestorm.Stems.Fuel.Resolving.Analysis
{
    /// <summary>
    /// Dictionaries with factories and predicates to be used in the Engine.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class EngineImplementations<TItem> // TODO rename?
        where TItem : class
    {
        public Dictionary<string, IFactory<IFieldResourceGetter<TItem>, TItem>> FullResourceFactories { get; } = new Dictionary<string, IFactory<IFieldResourceGetter<TItem>, TItem>>();
        public Dictionary<string, IFactory<IFieldReader<TItem>, TItem>> ReaderFactories { get; } = new Dictionary<string, IFactory<IFieldReader<TItem>, TItem>>();
        public Dictionary<string, IFactory<IFieldCollator<TItem>, TItem>> CollatorFactories { get; } = new Dictionary<string, IFactory<IFieldCollator<TItem>, TItem>>();
        public Dictionary<string, IFactory<IFieldWriter<TItem>, TItem>> WriterFactories { get; } = new Dictionary<string, IFactory<IFieldWriter<TItem>, TItem>>();
        public Dictionary<string, IFactory<IItemLocator<TItem>, TItem>> LocatorFactories { get; } = new Dictionary<string, IFactory<IItemLocator<TItem>, TItem>>();
        
        public Dictionary<string, IFactory<IIdentifierInfo<TItem>, TItem>> IdentifierFactories { get; } = new Dictionary<string, IFactory<IIdentifierInfo<TItem>, TItem>>();

        public Dictionary<string, IFieldDescription> Descriptions { get; } = new Dictionary<string, IFieldDescription>();
        public Dictionary<string, Func<IRestUser, bool>> AuthorizePredicates { get; } = new Dictionary<string, Func<IRestUser, bool>>();

        public Dictionary<Display, List<string>> Defaults { get; } = new Dictionary<Display, List<string>>();
    }
}