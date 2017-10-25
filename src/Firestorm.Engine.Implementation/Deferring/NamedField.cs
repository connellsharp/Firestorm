using System;
using Firestorm.Engine.Fields;

namespace Firestorm.Engine
{
    /// <summary>
    /// A reference to an item's field, as described by an API user.
    /// Reader is lazy-loaded.
    /// </summary>
    internal class NamedField<TItem> : INamedField<TItem>
        where TItem : class
    {
        private readonly Lazy<IFieldReader<TItem>> _lazyReader;
        private readonly Lazy<IFieldWriter<TItem>> _lazyWriter;

        internal NamedField(string name, IFieldProvider<TItem> provider)
        {
            _lazyReader = new Lazy<IFieldReader<TItem>>(() => provider.GetReader(name));
            _lazyWriter = new Lazy<IFieldWriter<TItem>>(() => provider.GetWriter(name));
            Name = name;
        }

        /// <summary>
        /// The name used to describe the field.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The field reader from the engine.
        /// </summary>
        public IFieldReader<TItem> Reader
        {
            get { return _lazyReader.Value; }
        }

        /// <summary>
        /// The field writer from the engine.
        /// </summary>
        public IFieldWriter<TItem> Writer
        {
            get { return _lazyWriter.Value; }
        }
    }
}