using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Fuel.Analysis;

namespace Firestorm.Stems.Essentials.Factories.Factories
{
    /// <summary>
    /// Creates new <see cref="BasicFieldCollator{TItem}"/> objects from the readers provided from the given reader factory.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    internal class BasicCollatorFactory<TItem> : IFactory<IFieldCollator<TItem>, TItem>
        where TItem : class
    {
        private readonly IFactory<IFieldReader<TItem>, TItem> _factory;

        public BasicCollatorFactory(IFactory<IFieldReader<TItem>, TItem> factory)
        {
            _factory = factory;
        }

        public IFieldCollator<TItem> Get(Stem<TItem> stem)
        {
            var reader = _factory.Get(stem);
            return new BasicFieldCollator<TItem>(reader);
        }
    }
}