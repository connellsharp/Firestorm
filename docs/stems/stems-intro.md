Introduction
------------

Firestorm Stems is a library built on top of the Firestorm Engine to provide a neat way to describe your RESTful API structure.

Stems are classes that contain the members used in your API. Members are decorated with Attributes that describe their usage.

## C# Stems

Stems describe how to handle an object of a specific type. Types can be Entities, business objects or DTOs. See [Best Practices](Tutorials/Stems/Best-Practices).

These two Stems describe the relationship between `Artist` and `Album` objects.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Get(Display.Nested)]
    [Identifier]
    public static int ID { get; }

    [Get, Set]
    public static string Name { get; }

    [Get(DisplayFor.Hidden)]
    [Substem(typeof(AlbumSubstem))]
    public static ICollection<Album> Albums { get; }
}

public class AlbumsStem : Stem<Album>
{
    [Get(Display.Nested)]
    [Identifier]
    public static int ID { get; }

    [Get, Set]
    public static string Title{ get; }

    [Get, Set]
    public static DateTime ReleaseDate { get; }

    [Get(DisplayFor.Hidden)]
    [Substem(typeof(TracksSubstem))]
    public static ICollection<Track> Tracks { get; }

    [Get]
    public static Expression<Func<Album, bool>> IsStreamable
    {
        get { return a => a.Streams.Any(s => s.IsAvailable); }
     }
}
```

## JSON REST API

The consumer can request certain fields in the artists collection.

``` request
GET /artists?fields=id,name,albums
```
``` json
200 OK
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

``` request
GET /artists/123/albums?fields=id,title&streamable=true&sort=release_date
```
``` json
200 OK
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

``` request
POST /artists/123/albums
{
    "title": "Dividing by Zero"
}
```
``` json
201 Created
{
    id: 9476
}
```

