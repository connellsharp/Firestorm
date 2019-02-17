using System;
using System.Reflection;
using Firestorm.Stems.Definitions;
using Reflectious;

namespace Firestorm.Stems.Analysis
{
    /// <summary>
    /// Analyzes stem types to build a <see cref="StemDefinition"/> model from members with custom <see cref="StemAttribute"/>s using their <see cref="IAttributeResolver"/> implementations.
    /// </summary>
    public class AttributeAnalyzer : IAnalyzer<StemDefinition, Type>
    {
        public void Analyze(StemDefinition destination, Type stemType)
        {
            Type stemBaseType = stemType.GetGenericSubclass(typeof(Stem<>))
                ?? throw new StemAttributeSetupException("Stem attributes applied to a class that does not derive from Stem<>.");

            Type itemType = stemBaseType.GetGenericArguments()[0];

            foreach (MemberInfo member in stemType.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
            {
                foreach (StemAttribute stemAttribute in member.GetCustomAttributes<StemAttribute>())
                {
                    IAttributeResolver resolver = stemAttribute.GetResolver();

                    resolver.Attribute = stemAttribute;
                    resolver.Definition = destination;
                    resolver.ItemType = itemType;

                    resolver.IncludeMember(member);
                }
            }
        }
    }
}