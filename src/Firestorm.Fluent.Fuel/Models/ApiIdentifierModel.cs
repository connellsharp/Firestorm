using Firestorm.Engine.Identifiers;

namespace Firestorm.Fluent.Fuel.Models
{
    internal class ApiIdentifierModel<TItem>
    {
        public IIdentifierInfo<TItem> IdentifierInfo { get; set; }

        public string Name { get; set; }
    }
}