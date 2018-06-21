using System;
using System.Collections.Generic;
using System.Reflection;

namespace Firestorm.Stems.Roots
{
    /// <summary>
    /// Gets types that are nested within a given parent type.
    /// </summary>
    public class NestedTypeGetter : ITypeGetter
    {
        private readonly Type _parentType;

        /// <summary>
        /// Gets types that are nested within the given <see cref="parentType"/>.
        /// </summary>
        public NestedTypeGetter(Type parentType)
        {
            _parentType = parentType;
        }

        public IEnumerable<Type> GetAvailableTypes()
        {
            return _parentType.GetNestedTypes(BindingFlags.Public);
        }
    }
}