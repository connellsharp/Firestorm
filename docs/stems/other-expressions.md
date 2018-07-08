# Other Expression Syntax Options

Writing out those basic single-property expressions can get pretty tedious. So there are other options.

## Expression() Method

The `Stem` base class provides a protected method that just returns the lambda expression given as its only argument. But this allows some type inference to kick in.

Firestorm doesn't use the declared type of the property, but it's return value. This means we can use `Expression` or `LambdaExpression` as the return type, to give it a cleaner look.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Get, Identifier]
    public static Expression Id => Expression(a => a.Id);

    [Get]
    public static Expression Name => Expression(a => a.Name);

    [Get]
    public static Expression StartDate => Expression(a => a.StartDate);
}
```

This way we haven't had to write out the type at all.

## Auto Mapping

Another option is to declare a property of the same signature as the property in the expression.

For safety, Firestorm requires the `AutoExpr` attribute in these cases.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Get, AutoExpr]
    public static string Id { get; }
    
    [Get, AutoExpr]
    public static string Name { get; }
    
    [Get, AutoExpr]
    public static DateTime StartDate { get; }
}
```

It's worth highlighting that these are purely descriptive and will be interpreted as `Expression` properties anyway. If you set the value of the above properties, it will be ignored.

Expressions will still be needed for more complex expressions. Those can be used alongside your auto mapped properties.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Get, AutoExpr]
    public static string ID { get; }
    
    [Get, AutoExpr]
    public static string Name { get; }

    [Get]
    public static Expression<Func<Album, bool>> HasStreamableAlbum
    {
        get { return a => a.Albums.Any(a => a.Streams.Any(s => s.IsAvailable)); }
    }
}
```

### Using your own mapping rules

You can extend or override the mapping rules in your `StemConfiguration` object.

You can create your own implementation of `IPropertyAutoMapper` and set is as the `IStemConfiguration.AutoPropertyMapper` property. For example, you could use your own rules from AutoMapper.

By default, this will use the `DefaultPropertyAutoMapper`. You could also use this as a fallback in your own implementation.

```csharp
public class DefaultPropertyAutoMapper : IPropertyAutoMapper
{
    public LambdaExpression MapExpression(PropertyInfo property, Type itemType)
    {
        var samePropertyName = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).FirstOrDefault(p => p.Name == property.Name);

        ParameterExpression paramExpression = Expression.Parameter(itemType);
        LambdaExpression expression = Expression.Lambda(Expression.Property(paramExpression, samePropertyName), paramExpression);
        return expression;
    }
}
```
