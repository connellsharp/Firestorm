# GetAttribute

The `Get` attribute marks a member as a field that an API client can read.

#### Static Expressions

Properties that return `LambdaExpression` are combined into parts of a query.

Static properties are loaded once on application startup, making them the most efficient field getters.

```csharp
public class ArtistsStem : Stem<Artist>
{
    [Get]
    public static Expression<Func<Artist, int>> Id
    {
        get { return a => a.Id; }
    }
    
    [Get]
    public static Expression<Func<Artist, string>> Name
    {
        get { return a => a.Name; }
    }
    
    [Get]
    public static Expression<Func<Artist, DateTime>> StartDate
    {
        get { return a => a.StartDate; }
    }
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
    "name": "Periphery",
    "start_date": "2005-01-01"
}
```

```http
GET /artists/123/name

200 OK
"Periphery"
```


#### Sorting and filtering

The Firestorm Engine provides [querying functionality](../endpoints/basic-requests.md).

```http
GET /artists?fields=id,name

200 OK
[                        
    {
        "id": 122,
        "name": "Noisia"
    },                                        
    {
        "id": 123,
        "name": "Periphery"
    },                                        
    {
        "id": 124,
        "name": "Infected Mushroom"
    }
]
```

```http
GET /artists?fields=id,name&sort=start_date

200 OK
[
    {
        "id": 124,
        "name": "Infected Mushroom"
    },
    {
        "id": 123,
        "name": "Periphery"
    },
    {
        "id": 122,
        "name": "Noisia"
    },
]
```

```http
GET /artists?fields=name&where=id>122

200 OK                
[       
    {
        "name": "Periphery"
    },
    {                
        "name": "Infected Mushroom"
    }
]
```

### Instance Getter Methods

You can also add the `Get` attribute to methods that return the type. This code is not used in the database query and is executed in your application.

Alone, this may not be very useful, but you can set the `Argument` property to pass in the value of an expression to make the value dynamic.

```csharp
public class ArtistsStem : Stem<Artist>
{
    [Get]
    public static Expression<Func<Artist, int>> Id
    {
        get { return a => a.Id; }
    }
    
    [Get]
    public static Expression<Func<Artist, DateTime>> StartDate
    {
        get { return a => a.StartDate; }
    }
    
    [Get(Argument = nameof(StartDate))]
    public static int GetYearsActive(DateTime startDate)
    {
        return (DateTime.Today - startDate).Years;
    }
}
```

```http
GET /artists/123

200 OK
{
    "id": 123,
    "start_date": "2005-01-01",
    "years_active": 13
}
```


# SetAttribute

The `Set` attribute marks a member as a field that an API client can write to.

#### Static Expressions

Simple properties that return `LambdaExpression` for a single property can also be used as setters.

```csharp
public class ArtistsStem : Stem<Artist>
{
    [Get]
    public static Expression<Func<Artist, int>> ID
    {
        get { return a => a.ID; }
    }
    
    [Get]
    [Set]
    public static Expression<Func<Artist, string>> Name
    {
        get { return a => a.Name; }
    }
    
    [Get]
    public static Expression<Func<Artist, DateTime>> StartDate
    {
        get { return a => a.StartDate; }
    }
}
```

```http
POST /artists
{
    "name": "Fred"
}

201 Created
```

```http
PUT /artists/123
{
    "name": "Fred"
}

200 OK
```

```http
PUT /artists/123/name
"Fred"

200 OK
```

#### Static Setter Methods

Void methods that take two parameters, one for the item and one for the field value, can also be used as setters for more complex scenarios. The attribute should be placed on a different method to the getter.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Get(Display.Nested)]
    public static Expression<Func<Artist, int>> ID
    {
        get { return a => a.ID; }
    }
    
    [Get]
    public static Expression<Func<Artist, string>> Name
    {
        get { return a => a.Name; }
    }

    [Set]
    public static void SetName(Artist artist, string name)
    {
        artist.OldName = artist.Name;
        artist.Name = name;
        artist.NameChangedDate = DateTime.Now;
    }
}
```

Because we prefix the method name with `Set`, Firestorm ignores the prefix and regonises both the expression and method as the same field in your API.

```http
POST /artists
{
    "name": "Fred"
}

201 Created
```

```http
PUT /artists/123
{
    "name": "Fred"
}

200 OK
```

#### Instance Setter Methods

Non-static methods of the same signature can be used too. These will use a single Stem instance per API request. This is very handy when combined with [Constructor Inection](dependency-injection.md).

```csharp
public class ArtistsStem : Stem<Artist>
{
    private readonly IArtistsService _service;

    public ArtistsStem(IArtistsService service)
    {
        _service = service;
    }

    [Get]
    public static Expression<Func<Artist, string>> Name
    {
        get { return a => a.Name; }
    }

    [Set]
    public void SetName(Artist artist, string name)
    {
        if(_service.ArtistExistsWithName(name))
            throw new ArgumentException("An artist already exists with this name.");
            
        artist.OldName = artist.Name;
        artist.Name = name;
        artist.NameChangedDate = DateTime.Now;
    }
}
```

```http
POST /artists
{
    "name": "The Beatles"
}

400 Bad Request
{
    "error": "argument",
    "message": "An artist already exists with this name."
}
```