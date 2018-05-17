# Introduction

Firestorm Stems is a library built on top of the Firestorm Engine to provide a neat way to describe your RESTful API structure.

Stems are classes that contain the members used in your API. Members are decorated with Attributes that describe their usage.

## C# Stems

Stems describe how to handle an object of a specific type. Types can be Entities, business objects or DTOs. See [Best Practices](Tutorials/Stems/Best-Practices).

These example Stems describe the relationship between `Artist` and `Album` objects.

```csharp
public class ArtistsStem : Stem<Artist>
{
    [Get, Identifier]
    public static Expression Id => Expression(a => a.Id);

    [Get, Set]
    public static Expression Name => Expression(a => a.Name);

    [Get(Display.Hidden)]
    [Substem(typeof(AlbumSubstem))]
    public static Expression Albums => Expression(a => a.Albums);
}

public class AlbumsStem : Stem<Album>
{
    [Get, Identifier]
    public static Expression Id => Expression(a => a.Id);

    [Get, Set]
    public static Expression Title => Expression(a => a.Title);

    [Get, Set]
    public static Expression ReleaseDate => Expression(a => a.ReleaseDate);

    [Get]
    public static Expression IsStreamable => Expression(a => a.Streams.Any(s => s.IsAvailable));
}
```

## JSON REST API

The consumer can request certain fields in the artists collection.

```http
GET /artists?fields=id,name,albums HTTP/1.1


HTTP/1.1 200 OK

[
    {
        "id": 123,
        "name": "Noisia",
        "albums": [
            { "id": 4321 },    
            { "id": 6344 },    
            { "id": 7653 }
        ]
    },            
    {
        "id": 981,
        "name": "Periphery",
        "albums": [
            { "id": 33276 },    
            { "id": 65478 }
        ]
    },
]
```

Or a list of albums by a specific artist.

```http
GET /artists/123/albums?fields=id,title&streamable=true&sort=release_date HTTP/1.1


HTTP/1.1 200 OK

[
    {
        "id": 4321,
        "title": "Split the Atom"
    },
    {
        "id": 6344,
        "title": "Purpose"
    },            
    {
        "id": 7653,
        "title": "Outer Edges"
    }
]
```

Add an album.

```http
POST /artists/123/albums HTTP/1.1

{
    "title": "Outer Edges Remixes"
}


HTTP/1.1 201 Created

{
    "id": 9476
}
```

And more. You can read more about the requests and responses [here](../endpoints/basic-requests.md).