using Firestorm.Engine.Fields;

namespace Firestorm.Fluent.Fuel
{
    internal class FieldImplementations<TItem>
    {
        public IFieldReader<TItem> Reader { get; set; }
        public IFieldWriter<object> Writer { get; set; }
        public IFieldDescription Description { get; set; }
    }
}