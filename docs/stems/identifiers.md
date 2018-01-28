IdentifierAttribute
-------------------

The `Identifier` attribute marks a member as a unique identifier for an item in the collection. Identifiers are used in URLs to navigate to a specific item.

#### Static Expressions

Static properties that return `Expression<Func<Artist, int>>` can be used to match the requested identifier in the URL.

This is particularly handy when the identifier is also a field.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Identifier]
    [Get(Display.Nested)]
    public static int ID { get; }
    
    [Get]
    public static string Name { get; }
    
    [Get]
    public static DateTime StartDate { get; }
}
```

``` json
GET /artists
200 OK
[        
    { "id": 122 },
    { "id": 123 },
    { "id": 124 }
]
```

``` json
GET /artists/123
200 OK
{
    "id": 123,
    "name": "Wattitude"
}
```

``` json
GET /artists/by_id/123
200 OK
{
    "id": 123,
    "name": "Wattitude"
}
```

#### Getter Methods

Methods that take a single argument and return an item can also be used directly. Firestorm will attempt to convert the given value into the parameter type. The method can return null if no item with the given identifier was found.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Identifier]
    [Get(Display.Nested)]
    public static int ID { get; }

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

``` json
GET /artists/noisia
200 OK
{
    "id": 124,
    "name": "Noisia"
}
```

``` json
GET /artists/by_name/noisia
200 OK
{                
    "id": 124,
    "name": "Noisia"
}
```

``` json
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

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Identifier]
    [Get(Display.Nested)]
    public static int ID { get; }

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

``` json
GET /artists/noisia
200 OK
{
    "id": 124,
    "name": "Noisia"
}
```

```
GET /artists/by_name/noisia
200 OK
{                
    "id": 124,
    "name": "Noisia"
}
```

When multiple identifiers are used, these are matched first and therefore override any identifiers that match the same string. In the above example, this means any artist with the name "me" cannot be identified by name.