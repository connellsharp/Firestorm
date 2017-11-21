using Firestorm.Fluent.Fuel.Definitions;
using Firestorm.Fluent.Fuel.Sources;

namespace Firestorm.Fluent.Fuel.Builder
{
    public class EngineApiBuilder : IApiBuilder
    {
        public EngineApiBuilder(EngineApiModel model)
        {
            Model = model;
        }

        public EngineApiModel Model { get; set; }

        public virtual IApiItemBuilder<TItem> Item<TItem>()
            where TItem : class
        {
            var itemModel = new ApiItemModel<TItem>();
            var itemBuilder = new EngineItemBuilder<TItem>(itemModel);
            Model.Items.Add(itemModel.RootName, itemModel);
            return itemBuilder;
        }

        public IApiDirectorySource BuildSource()
        {
            return new EngineDirectorySource(Model);
        }
    }
}