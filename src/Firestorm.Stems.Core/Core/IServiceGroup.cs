using System;
using System.Collections.Generic;

namespace Firestorm.Stems
{   
    /// <summary>
    /// A set of services grouped by a derived Stem <see cref="Type"/>.
    /// </summary>
    public interface IServiceGroup
    {
        void Preload(IEnumerable<Type> stemTypes);
        IServiceProvider GetProvider(Type stemType);
    }
}