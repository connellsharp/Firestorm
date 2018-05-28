using System;
using System.Reflection;

namespace Firestorm.Stems.Roots
{
    internal class AttributedTypeValidator : ITypeValidator
    {
        private readonly Type _attributeType;
        private readonly bool _shouldHaveAttribute;

        public AttributedTypeValidator(Type attributeType, bool shouldHaveAttribute)
        {
            _attributeType = attributeType;
            _shouldHaveAttribute = shouldHaveAttribute;
        }

        public bool IsValidType(Type type)
        {
            bool hasCustomAttribute = type.GetCustomAttribute(_attributeType) != null;
            return hasCustomAttribute == _shouldHaveAttribute;
        }
    }
}