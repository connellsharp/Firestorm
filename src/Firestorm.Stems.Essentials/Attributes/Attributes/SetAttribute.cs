using System;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Essentials.Resolvers;

namespace Firestorm.Stems.Essentials
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

        public override Analysis.IAttributeResolver GetResolver()
        {
            return new SetterAttributeResolver();
        }
    }
}