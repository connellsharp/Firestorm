using Firestorm.Engine.Fields;

namespace Firestorm.Engine.Tests.Models
{
    public class Field<T>
        where T : class
    {
        public IFieldReader<T> Reader { get; set; }

        public IFieldCollator<T> Collator { get; set; }
        
        public IFieldWriter<T> Writer { get; set; }
    }
}