using System;
using System.Reflection;

namespace Firestorm.Stems.Roots
{
    internal class AttributedSuffixedDerivedTypeDictionary : SuffixedDerivedTypeDictionary
    {
        private readonly Type _attributeType;

        public AttributedSuffixedDerivedTypeDictionary(Type type, string root, Type attributeType)
            :base(type, root)
        {
            _attributeType = attributeType;
        }

        protected override bool IsValidType(Type type)
        {
            return type.GetCustomAttribute(_attributeType) != null && base.IsValidType(type);
        }
    }
}