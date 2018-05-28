using System;

namespace Firestorm.Stems.Roots
{
    /// <summary>
    /// Confirms whether a type is valid or not to be included in a <see cref="NamedTypeDictionary"/>.
    /// </summary>
    public interface ITypeValidator
    {
        bool IsValidType(Type type);
    }
}