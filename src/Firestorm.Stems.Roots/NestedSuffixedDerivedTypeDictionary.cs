using System;
using System.Collections.Generic;
using System.Reflection;

namespace Firestorm.Stems.Roots
{
    internal class NestedSuffixedDerivedTypeDictionary : SuffixedDerivedTypeDictionary
    {
        private readonly Type _parentType;

        public NestedSuffixedDerivedTypeDictionary(Type parentType, Type baseType, string suffix)
            : base(baseType, suffix)
        {
            _parentType = parentType;
        }


        protected override IEnumerable<Type> FindAllValidTypes()
        {
            return _parentType.GetNestedTypes(BindingFlags.Public);
        }
    }
}