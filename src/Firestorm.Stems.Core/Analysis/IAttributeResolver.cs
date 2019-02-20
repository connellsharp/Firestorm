using System;
using System.Reflection;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Definitions;

namespace Firestorm.Stems.Analysis
{
    /// <summary>
    /// Adds to a <see cref="StemDefinition"/> with information about a Stem member with a <see cref="StemAttribute"/>.
    /// </summary>
    public interface IAttributeResolver
    {
        Type ItemType { set; }

        StemDefinition Definition { set; }

        StemAttribute Attribute { set; }
        
        IPropertyAutoMapper PropertyAutoMapper { set; }

        void IncludeMember(MemberInfo member);
    }
}