using System;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Essentials.Resolvers;

namespace Firestorm.Stems.Essentials
{
    /// <summary>
    /// Marks a locator field that can be used to find an item in a Substem instead of setting the value of another.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class LocatorAttribute : FieldAttribute
    {
        public LocatorAttribute()
        { }

        public LocatorAttribute(string name)
            : base(name)
        { }


        public override string GetDefaultName(string memberName)
        {
            return memberName.TrimStart("FindBy");
        }

        public override IAttributeResolver GetResolver()
        {
            return new LocatorAttributeResolver();
        }
    }
}