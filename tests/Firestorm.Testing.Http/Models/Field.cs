using Firestorm.Engine.Fields;

namespace Firestorm.Tests.Integration.Http.Base.Models
{
    // TODO all these were taking from the Engine Unit tests. Probably should stay here and be removed from the unit tests?

    public class Field<T>
        where T : class
    {
        public IFieldReader<T> Reader { get; set; }
        
        public IFieldCollator<T> Collator { get; set; }
        
        public IFieldWriter<T> Writer { get; set; }
    }
}