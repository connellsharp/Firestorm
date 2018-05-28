using System;

namespace Firestorm.Stems.Roots
{
    internal class TypeDictionaryPopulator
    {
        private readonly NamedTypeDictionary _dictionary;
        private readonly ITypeValidator _validator;

        public TypeDictionaryPopulator(NamedTypeDictionary dictionary, ITypeValidator validator)
        {
            _dictionary = dictionary;
            _validator = validator;
        }

        public void AddTypeEnsuringValid(Type type)
        {
            if (!_validator.IsValidType(type))
                throw new StemStartSetupException(type.Name + " is not a valid type to use in this dictionary.");

            _dictionary.AddType(type);
        }

        public void AddValidTypes(ITypeGetter typeGetter)
        {
            foreach (Type type in typeGetter.GetAvailableTypes())
            {
                if (_validator.IsValidType(type))
                    _dictionary.AddType(type);
            }
        }
    }
}
