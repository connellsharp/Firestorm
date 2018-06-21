using System;
using System.Collections.Generic;

namespace Firestorm.Extensions.AspNetCore
{
    internal interface IItemTypeFinder
    {
        IEnumerable<Type> FindItemTypes();
    }
}