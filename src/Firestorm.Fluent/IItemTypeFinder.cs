using System;
using System.Collections.Generic;

namespace Firestorm.Fluent
{
    internal interface IItemTypeFinder
    {
        IEnumerable<Type> FindItemTypes();
    }
}