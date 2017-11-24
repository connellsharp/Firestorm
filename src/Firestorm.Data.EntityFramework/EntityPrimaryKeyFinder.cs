using System;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;
using System.Reflection;

namespace Firestorm.Data.EntityFramework
{
    /// <remarks>
    /// Taken from http://stackoverflow.com/a/3046102
    /// </remarks>
    internal class EntityPrimaryKeyFinder : IPrimaryKeyFinder
    {
        public T GetByPrimaryKey<T>(ObjectContext database, int id)
            where T : class
        {
            return (T)database.GetObjectByKey(new EntityKey(database.DefaultContainerName + "." + GetTableName<T>(), GetPrimaryKeyInfo<T>().Name, id));
        }

        public string GetTableName<T>()
        {
            return GetTableName(typeof(T));
        }

        public string GetTableName(Type entityType)
        {
            return Pluralize(entityType.Name);
        }

        public static string Pluralize(string singular)
        {
            const StringComparison comp = StringComparison.InvariantCultureIgnoreCase;

            if (singular.ToLower() == "person")
                return "People";

            if (singular.ToLower() == "tooth")
                return "Teeth";

            if (singular.EndsWith("y", comp))
                return singular.Remove(singular.Length - 1, 1) + "ies";

            if (singular.EndsWith("f", comp))
                return singular.Remove(singular.Length - 1, 1) + "ves";

            if (singular.EndsWith("fe", comp))
                return singular.Remove(singular.Length - 2, 2) + "ves";

            if (singular.EndsWith("s", comp) || singular.EndsWith("ch", comp) || singular.EndsWith("x", comp) || singular.EndsWith("z", comp) || singular.EndsWith("sh", comp))
                return singular + "es";

            return singular + "s";
        }

        public PropertyInfo GetPrimaryKeyInfo<T>()
        {
            return GetPrimaryKeyInfo(typeof(T));
        }

        public PropertyInfo GetPrimaryKeyInfo(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo pI in properties)
            {
                object[] attributes = pI.GetCustomAttributes(true);
                foreach (object attribute in attributes)
                {
                    var propertyAttribute = attribute as EdmScalarPropertyAttribute;
                    if (propertyAttribute != null)
                    {
                        if (propertyAttribute.EntityKeyProperty)
                            return pI;
                    }
                    else
                    {
                        var columnAttribute = attribute as ColumnAttribute;
                        if (columnAttribute != null && columnAttribute.IsPrimaryKey)
                            return pI;
                    }
                }
            }

            return null;
        }

        public Expression<Func<TEntity, TProperty>> GetPrimaryKeyExpression<TEntity, TProperty>()
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity));
            PropertyInfo propertyInfo = GetPrimaryKeyInfo<TEntity>();
            return Expression.Lambda<Func<TEntity, TProperty>>(Expression.Property(param, propertyInfo), param);
        }
    }
}