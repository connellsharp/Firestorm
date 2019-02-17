using System;

namespace Firestorm.Stems
{
    public interface IImplementationResolver
    {        
        T Get<T>(Type stemType);
    }
}