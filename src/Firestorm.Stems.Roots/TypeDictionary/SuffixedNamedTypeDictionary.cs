using System;

namespace Firestorm.Stems.Roots
{
    public class SuffixedNamedTypeDictionary : NamedTypeDictionary
    {
        private readonly string _nameSuffix;

        public SuffixedNamedTypeDictionary(string nameSuffix)
        {
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
    }
}