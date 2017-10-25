using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm.Stems
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