using System;
using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Basic.Resolvers;

namespace Firestorm.Stems
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class SetAttribute : FieldAttribute
    {
        public SetAttribute()
        { }

        public SetAttribute(string name)
            : base(name)
        { }

        public string Getter { get; set; }

        public override string GetDefaultName(string memberName)
        {
            return memberName.TrimStart("Set");
        }

        public override IAttributeResolver GetResolver()
        {
            return new SetterAttributeResolver();
        }
    }
}