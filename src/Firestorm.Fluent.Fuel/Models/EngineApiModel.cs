using System.Collections.Generic;

namespace Firestorm.Fluent.Fuel.Models
{
    public class EngineApiModel
    {
        public IList<IApiItemModel> Items { get; } = new List<IApiItemModel>();
    }
}