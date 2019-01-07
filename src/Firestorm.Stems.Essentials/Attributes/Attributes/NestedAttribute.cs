using System;

namespace Firestorm.Stems.Essentials
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class NestedAttribute : StemHelperAttribute
    {
        public NestedAttribute()
            : this(1)
        {
        }

        public NestedAttribute(int nestingLevel)
        {
            NestingLevel = nestingLevel;
        }

        public int NestingLevel { get; }
    }
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class HiddenAttribute : NestedAttribute
    {
        public HiddenAttribute()
            : base(-1)
        {
        }
    }
}