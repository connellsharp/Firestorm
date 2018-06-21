using System;

namespace Firestorm.Stems.Roots
{
    public class DerivedTypeValidator : ITypeValidator
    {
        private readonly Type _baseType;

        public DerivedTypeValidator(Type baseType)
        {
            _baseType = baseType;
        }

        public bool IsValidType(Type type)
        {
            return _baseType.IsAssignableFrom(type);
        }
    }
}