using Firestorm.Engine;

namespace Firestorm.Fluent.Fuel
{
    public class FluentCollectionCreator<TItem> : IFluentCollectionCreator
        where TItem : class
    {
        public IRestCollection GetRestCollection()
        {
            IEngineContext<TItem> context = new FluentEngineContext<TItem>();
            return new EngineRestCollection<TItem>(context);
        }
    }
}