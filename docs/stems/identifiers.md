# IdentifierAttribute

The `Identifier` attribute marks a member as a unique identifier for an item in the collection. Identifiers are used in URLs to navigate to a specific item.

#### Static Expressions

Properties that return a `LambdaExpression` can be used to match the requested identifier in the URL.

This is particularly handy when the identifier is also a field.

```csharp
public class ArtistsStem : Stem<Artist>
{
    [Identifier]
    [Get]
    public static int Id { get; }
    
    [Get]
    public static string Name { get; }
    
    [Get]
    public static DateTime StartDate { get; }
}
```

```http
GET /artists

200 OK
[        
    { "id": 122 },
    { "id": 123 },
    { "id": 124 }
]
```

```http
GET /artists/123

200 OK
{
    "id": 123,
    "name": "Periphery"
}
```

```http
GET /artists/by_id/123

200 OK
{
    "id": 123,
    "name": "Periphery"
}
```

#### Getter Methods

Methods that take a single argument and return an item can also be used directly. Firestorm will attempt to convert the given value into the parameter type. The method can return null if no item with the given identifier was found.

```csharp
public class ArtistsStem : Stem<Artist>
{
    [Identifier]
    [Get]
    public static int Id { get; }

    [Identifier]
    public Artist FindByName(string name)
    {
        name = name.Replace("_", " ");
        return _artistsService.FindByName(name);
    }
    
    [Get]
    public static string Name { get; }
    
    [Get]
    public static DateTime StartDate { get; }
}
```

```http
GET /artists/noisia

200 OK
{
    "id": 124,
    "name": "Noisia"
}
```

```http
GET /artists/by_name/noisia

200 OK
{                
    "id": 124,
    "name": "Noisia"
}
```

```http
GET /artists/by_id/124

200 OK
{                
    "id": 124,
    "name": "Noisia"
}
```

When multiple identifiers are defined but not specified in the URL, both identifiers are searched. If matches are found in both, an exception is thrown.

#### Special identifier properties

Properties or parameterless methods that return an item can be used for special identifier strings.

```csharp
public class ArtistsStem : Stem<Artist>
{
    [Identifier]
    [Get]
    public static int Id { get; }

    [Identifier]
    public Artist FindByName(string name)
    {
        name = name.Replace("_", " ");
        return _artistsService.FindByName(name);
    }

    [Identifier(Exactly = "me"]
    public Artist GetMyArtist()
    {
        if(User.IsArtist)
        {
            return _artistService.GetArtistByID(User.ArtistID);
        }

        return null;
    }
    
    [Get]
    public static string Name { get; }
    
    [Get]
    public static DateTime StartDate { get; }
}
```

```http
GET /artists/me

200 OK
{
    "id": 7654,
    "name": "My Awesome Band"
}
```

When multiple identifiers are used, these are matched first and therefore override any identifiers that match the same string. In the above example, this means any artist with the name "me" cannot be identified by name unless the `by_name` path is used.