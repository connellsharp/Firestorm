using Firestorm.Fluent.Fuel.Models;
using Firestorm.Fluent.Fuel.Sources;
using Firestorm.Fluent.Sources;

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
            where TItem : class, new()
        {
            var itemModel = new ApiItemModel<TItem>(Model.DataSource);
            var itemBuilder = new EngineItemBuilder<TItem>(itemModel);
            Model.Items.Add(itemModel);
            return itemBuilder;
        }

        public IApiDirectorySource BuildSource()
        {
            return new EngineDirectorySource(Model);
        }
    }
}