using System.Collections.Generic;
using Firestorm.Engine.Fields;

namespace Firestorm.Fluent.Fuel
{
    internal class FieldImplementationsDictionary<TItem> : Dictionary<string, FieldImplementations<TItem>>
    { }

    internal class FieldImplementations<TItem>
    {
        public IFieldReader<TItem> Reader { get; set; }
        public IFieldWriter<object> Writer { get; set; }
        public IFieldDescription Description { get; set; }
    }
}