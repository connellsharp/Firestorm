using System.Collections.Generic;

namespace Firestorm.Fluent.Fuel.Models
{
    public class EngineApiModel
    {
        public IDictionary<string, IApiItemModel> Items { get; } = new Dictionary<string, IApiItemModel>();
    }
}