using System.Reflection;
using Firestorm.Data;

namespace Firestorm.Engine.Additives.Identifiers
{
    /// <summary>
    /// An identifier found automatically using Entity Framework's primary key.
    /// </summary>
    public class PrimaryKeyIdentifierInfo<TItem> : PropertyInfoIdentifierInfo<TItem>
    {
        public PrimaryKeyIdentifierInfo(IPrimaryKeyFinder keyfinder)
            : base(GetPropertyInfo(keyfinder))
        {
        }

        private static PropertyInfo GetPropertyInfo(IPrimaryKeyFinder keyfinder)
        {
            return keyfinder.GetPrimaryKeyInfo(typeof(TItem))
                   ?? IdConventionIdentifierInfo<TItem>.GetKeyPropertyByName();
        }
    }
}