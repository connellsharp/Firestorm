using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Firestorm.Stems.AutoMap
{
    public class DefaultPropertyAutoMapper : IPropertyAutoMapper
    {
        public LambdaExpression MapExpression(PropertyInfo property, Type itemType)
        {
            var samePropertyName = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).FirstOrDefault(p => p.Name == property.Name);
            if (samePropertyName == null)
                throw new InvalidOperationException("No properties exist on the item class with the name '" + property.Name + "'");

            if (samePropertyName.PropertyType != property.PropertyType)
                throw new InvalidOperationException("Cannot auto-map property '" + property.Name + "' because the item class' property with the same name does not return the same type.");

            ParameterExpression paramExpression = Expression.Parameter(itemType);
            LambdaExpression expression = Expression.Lambda(Expression.Property(paramExpression, samePropertyName), paramExpression);
            return expression;
        }

        // TODO AutoMapper integration? Maybe inject into config somehow?
        //IConfigurationProvider mapperConfig = new MapperConfiguration(cfx => cfx.CreateMap(property.ReflectedType, ItemType));
        //PropertyMap propertyMap = mapperConfig.GetPropertyMap(property, ItemType);
        //LambdaExpression expression = propertyMap.CustomExpression;
    }
}