using System;

namespace Firestorm.Stems.Roots
{
    public class SuffixValidator : ITypeValidator
    {
        private readonly string _suffix;

        public SuffixValidator(string suffix)
        {
            _suffix = suffix;
        }

        public bool IsValidType(Type type)
        {
            return type.Name.EndsWith(_suffix);
        }
    }
}