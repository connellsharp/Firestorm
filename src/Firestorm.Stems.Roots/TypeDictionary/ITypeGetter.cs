using System;
using System.Collections.Generic;

namespace Firestorm.Stems.Roots
{
    public interface ITypeGetter
    {
        IEnumerable<Type> GetAvailableTypes();
    }
}