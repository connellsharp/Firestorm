using System;
using System.Reflection;
using Firestorm.Stems.Attributes.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Reflectious;

namespace Firestorm.Stems.Attributes.Analysis
{
    /// <summary>
    /// Analyzes stem types to build a <see cref="StemDefinition"/> model from members with custom <see cref="StemAttribute"/>s using their <see cref="IAttributeResolver"/> implementations.
    /// </summary>
    public class AttributeAnalyzer : IAnalyzer
    {
        public AttributeAnalyzer()
        {
            Definition = new StemDefinition();
        }

        public StemDefinition Definition { get; }

        public void Analyze(Type stemType, IStemConfiguration configuration)
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
                    resolver.Definition = Definition;
                    resolver.ItemType = itemType;
                    resolver.Configuration = configuration;

                    resolver.IncludeMember(member);
                }
            }
        }
    }
}