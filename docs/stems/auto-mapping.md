# Auto Mapping Properties

Writing out those basic single-property expressions can get pretty tedious. So there's another option.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Get(Display.Nested)]
    public static string ID { get; }
    
    [Get]
    public static string Name { get; }
    
    [Get]
    public static DateTime StartDate { get; }
}
```

It's worth noting that these are purely descriptive and will be interpreted as `Expression` properties anyway. If you set the value of the above properties, it will be ignored.

Expressions will always be needed for more complex expressions. Those can be combined with your auto mapped properties.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Get(Display.Nested)]
    public static string ID { get; }
    
    [Get]
    public static string Name { get; }

    [Get]
    public static Expression<Func<Album, bool>> HasStreamableAlbum
    {
        get { return a => a.Albums.Any(a => a.Streams.Any(s => s.IsAvailable)); }
    }
}
```

## Using your own mapping rules

You can extend or override the mapping rules in your `StemConfiguration` object.

You can create your own implementation of `IPropertyAutoMapper` and set is as the `IStemConfiguration.AutoPropertyMapper` property. For example, you could use your own rules from AutoMapper.

By default, this will use the `DefaultPropertyAutoMapper`. You could also use this as a fallback in your own implementation.

```csharp
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
}
```
