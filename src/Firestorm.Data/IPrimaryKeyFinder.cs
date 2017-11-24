using System;
using System.Reflection;

namespace Firestorm.Data
{
    public interface IPrimaryKeyFinder
    {
        PropertyInfo GetPrimaryKeyInfo(Type type);
    }
}