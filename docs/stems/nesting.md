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

## Defaults

If no `Display` argument is given, the default is usually `Display.FullItem`. However, if the name matches the ID convention rules, `Display.Nested` is used by default.