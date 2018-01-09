using System.Linq;
using Firestorm.Data;
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

        public EngineApiModel Model { get; }

        public virtual IApiItemBuilder<TItem> Item<TItem>()
            where TItem : class, new()
        {
            var itemModel = Model.Roots.FirstOrDefault(i => i.ItemType == typeof(TItem)) as ApiItemModel<TItem>
                            ?? CreateNewItemModel<TItem>();

            var itemBuilder = new EngineItemBuilder<TItem>(itemModel);
            return itemBuilder;
        }

        private ApiItemModel<TItem> CreateNewItemModel<TItem>()
            where TItem : class, new()
        {
            var itemModel = new ApiItemModel<TItem>(Model.DataSource);

            Model.Items.Add(itemModel);
            Model.Roots.Add(itemModel);

            itemModel.RootName = PluralConventionUtility.Pluralize(typeof(TItem).Name);

            return itemModel;
        }

        public IApiDirectorySource BuildSource()
        {
            return new EngineDirectorySource(Model);
        }
    }
}