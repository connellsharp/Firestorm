using System;

namespace Firestorm.Stems.Roots
{
    public class SuffixedDerivedTypeDictionary : NamedTypeDictionary
    {
        private readonly Type _baseType;
        private readonly string _nameSuffix;

        public SuffixedDerivedTypeDictionary(Type baseType, string nameSuffix)
        {
            _baseType = baseType;
            _nameSuffix = nameSuffix;
        }

        protected override string GetName(Type type)
        {
            string name = type.Name;

            int index = name.IndexOf('`');
            if (index != -1)
                name = name.Substring(0, index);

            return name.TrimEnd(_nameSuffix);
        }

        protected override bool IsValidType(Type type)
        {
            return _baseType.IsAssignableFrom(type);
        }
    }
}