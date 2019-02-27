using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Data;

namespace Firestorm.EntityFramework6
{
    /// <remarks>
    /// Taken from http://stackoverflow.com/a/3046102
    /// </remarks>
    internal class EntityPrimaryKeyFinder : IPrimaryKeyFinder
    {
        public T GetByPrimaryKey<T>(ObjectContext database, int id)
            where T : class
        {
            return (T)database.GetObjectByKey(new EntityKey(database.DefaultContainerName + "." + PluralConventionUtility.GetTableName<T>(), GetPrimaryKeyInfo<T>().Name, id));
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
                    if (attribute is EdmScalarPropertyAttribute propertyAttr && propertyAttr.EntityKeyProperty)
                        return pI;

                    // TODO reintroduce System.Data.Linq.Mapping.ColumnAttribute
                    //if (attribute is ColumnAttribute columnAttr && columnAttr.IsPrimaryKey)
                    //    return pI;
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