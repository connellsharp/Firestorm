using Firestorm.Engine.Fields;

namespace Firestorm.Fluent.Fuel.Definitions
{
    internal class ApiFieldModel<TItem>
        where TItem : class
    {
        public string Name { get; set; }
        public IFieldReader<TItem> Reader { get; set; }
        public IFieldWriter<TItem> Writer { get; set; }
        public IFieldDescription Description { get; set; }
    }
}