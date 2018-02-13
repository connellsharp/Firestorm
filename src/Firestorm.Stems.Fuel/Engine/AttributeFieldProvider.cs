using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Writers;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Context;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems.Fuel.Fields
{
    /// <summary>
    /// Fields provider built using reflection to look at <see cref="FieldAttribute"/>s on a class.
    /// </summary>
    internal class AttributeFieldProvider<TItem> : ILocatableFieldProvider<TItem>
        where TItem : class
    {
        private readonly Stem<TItem> _stem;
        private readonly EngineImplementations<TItem> _implementations;

        public AttributeFieldProvider(Stem<TItem> stem)
        {
            _stem = stem;

            var analyzerFactory = stem.Configuration.AnalyzerCache;
            var definitionAnalyzer = analyzerFactory.GetAnalyzer<FieldDefinitionAnalyzer<TItem>>(stem.GetType(), stem.Configuration);
            _implementations = definitionAnalyzer.Implementations;
        }

        public IEnumerable<string> GetDefaultNames(int nestedBy)
        {
            do
            {
                if (_implementations.Defaults.ContainsKey((Display) nestedBy))
                    return _implementations.Defaults[(Display) nestedBy];
            } while (nestedBy-- >= -1);

            return Enumerable.Empty<string>();
        }

        public IRestResource GetFullResource(string fieldName, IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            if (!_implementations.FullResourceFactories.ContainsKey(fieldName))
                return null; // TODO scalar?

            return _implementations.FullResourceFactories[fieldName].Get(_stem).GetFullResource(item, dataTransaction);
        }

        public IFieldReader<TItem> GetReader(string fieldName)
        {
            if (!_implementations.ReaderFactories.ContainsKey(fieldName))
                return null;

            return _implementations.ReaderFactories[fieldName].Get(_stem);
        }

        public IFieldWriter<TItem> GetWriter(string fieldName)
        {
            if (_implementations.WriterFactories.ContainsKey(fieldName))
                return _implementations.WriterFactories[fieldName].Get(_stem);

            IFieldReader<TItem> reader = GetReader(fieldName);
            if (reader != null)
                return new ConfirmOnlyFieldWriter<TItem>(fieldName, reader);

            return null;
        }

        public IItemLocator<TItem> GetLocator(string fieldName)
        {
            if (!_implementations.LocatorFactories.ContainsKey(fieldName))
                return null;

            return _implementations.LocatorFactories[fieldName].Get(_stem);
        }

        public IFieldDescription GetDescription(string fieldName, CultureInfo cultureInfo)
        {
            // TODO ignoring cultureinfo here?
            return _implementations.Descriptions[fieldName];
        }

        public bool FieldExists(string fieldName)
        {
            // TODO: just readers?
            return _implementations.ReaderFactories.ContainsKey(fieldName);
        }
    }
}