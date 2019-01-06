using Firestorm.Stems.Attributes.Attributes;
using JetBrains.Annotations;

namespace Firestorm.Stems.Attributes.Basic.Attributes
{
    [MeansImplicitUse]
    public abstract class FieldAttribute : StemAttribute
    {
        protected FieldAttribute()
        { }

        protected FieldAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public virtual string GetDefaultName(string memberName)
        {
            return memberName;
        }
    }
}