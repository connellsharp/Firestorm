# Substems

Substems are used to define the relationships between objects.

You add the `Substem` attribute to navigation properties, supplying the `Stem<>` type that defines the fields for the navigation object(s).

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Identifier]
    [Get(Display.Nested)]
    public static int ID { get; }
    
    [Get]
    public static string Name { get; }
    
    [Get]
    [Substem(typeof(AlbumStem))]
    public static IEnumerable<Album> Albums { get; }
}

[DataSourceRoot]
public class AlbumsStem : Stem<Album>
{
    [Identifier]
    [Get(Display.Nested)]
    public static int ID { get; }
            
    [Get]
    [Substem(typeof(TracksStem))]
    public static IEnumerable<Track> Tracks { get; }
}

[DataSourceRoot]
public class TracksStem : Stem<Track>
{
    [Identifier]
    [Get(Display.Nested)]
    public static int ID { get; }
            
    [Get]
    [Substem(typeof(TracksStem))]
    public static string Title { get; }
}
```

These define the paths used to navigate the API.

``` json
GET /artists/123/tracks
200 OK
[        
    { "id": 12345},
    { "id": 12346},
    { "id": 12347}
]
```

``` json
GET /artists/123/tracks/by_index/1
200 OK
{
    "id": 12345,
    "title": "Awesome Song"
}
```