using System;

namespace Firestorm.Stems
{   
    /// <summary>
    /// A set of services grouped by a derived Stem <see cref="Type"/>.
    /// </summary>
    public interface IServiceGroup
    {        
        IServiceProvider GetProvider(Type stemType);
    }
}