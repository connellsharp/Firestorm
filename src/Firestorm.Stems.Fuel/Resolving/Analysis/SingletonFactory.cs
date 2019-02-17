using Firestorm.Engine.Fields;

namespace Firestorm.Stems.Fuel.Resolving.Analysis
{
    /// <summary>
    /// A factory that returns the same instance of an <see cref="IFieldHandler{TItem}"/> implementation, regardless of which stem instance is used.
    /// </summary>
    public class SingletonFactory<THandler, TItem> : IFactory<THandler, TItem>
        where THandler : IFieldHandler<TItem>
        where TItem : class
    {
        private readonly THandler _singletonHandler;

        public SingletonFactory(THandler singletonHandler)
        {
            _singletonHandler = singletonHandler;
        }

        public THandler Get(Stem<TItem> stem)
        {
            return _singletonHandler;
        }
    }
}