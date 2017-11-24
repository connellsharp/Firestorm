using System.Reflection;
using Firestorm.Engine.Additives.Identifiers;

namespace Firestorm.Data.EntityFramework.PrimaryKey
{
    /// <summary>
    /// An identifier found automatically using Entity Framework's primary key.
    /// </summary>
    public class PrimaryKeyIdentifierInfo<TItem> : PropertyInfoIdentifierInfo<TItem>
    {
        public PrimaryKeyIdentifierInfo()
            : base(GetPropertyInfo())
        {
        }

        private static PropertyInfo GetPropertyInfo()
        {
            return PrimaryKeyUtility.GetPrimaryKeyInfo<TItem>()
                   ?? IDConventionIdentifierInfo<TItem>.GetKeyPropertyByName();
        }
    }
}