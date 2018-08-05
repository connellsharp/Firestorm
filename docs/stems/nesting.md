# GetAttribute

Get attributes can be given a `Display.Nested` argument to display the field even when nested in a collection.

```csharp
public class ArtistsStem : Stem<Artist>
{
    [Get(Display.Nested)]
    public static Expression Id => Expression(a => a.Id);

    [Get(Display.Nested)]
    public static Expression Name => Expression(a => a.Name);

    [Get]
    public static Expression StartDate => Expression(a => a.StartDate);
}
```

```http
GET /artists

200 OK
[        
    { "id": 122, "name": "Noisia" },
    { "id": 123, "name": "Periphery" },
    { "id": 124, "name": "Infected Mushroom" }
]
```

## Hidden fields

The reverse of this is to use `Display.Hidden`, which means the field won't be displayed even when the full item is requested.

```csharp
public class ArtistsStem : Stem<Artist>
{
    [Get]
    public static Expression Id => Expression(a => a.Id);

    [Get]
    public static Expression Name => Expression(a => a.Name);

    [Get(Display.Hidden)]
    public static Expression StartDate => Expression(a => a.StartDate);
}
```

```http
GET /artists/123

200 OK
{ "id": 123, "name": "Periphery" }
```

Hidden feilds can only be accessed directly using the URL or by specifiying them explicitly in the querystring.

```http
GET /artists/123?fields=id,start_date

200 OK
{ "id": 123, "start_date": "2005-01-01" }
```

## Defaults

If no `Display` argument is given, the default is usually `Display.FullItem`. However, if the name matches the ID convention rules, `Display.Nested` is used by default.


# Attribute alternatives

As a small bit of syntactic sugar, you may prefer to use the `Nested` or `Hidden` attributes instead.

These don't provide any functionality on their own, but can be used alongside a `Get` attribute.

```csharp
public class ArtistsStem : Stem<Artist>
{
    [Get]
    [Nested]
    public static Expression Id => Expression(a => a.Id);

    [Get]
    [Nested]
    public static Expression Name => Expression(a => a.Name);

    [Get]
    public static Expression StartDate => Expression(a => a.StartDate);
}
```

Exceprtions will be thrown if multiple ways of setting the nesting level are used for the same field.