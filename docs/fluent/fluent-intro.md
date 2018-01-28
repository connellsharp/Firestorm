# Fluent API

The Firestorm Fluent API is a more compact way to describe your REST API using a builder API.

It is more suitable for simpler REST APIs. Unlike Stems, there is no custom type to inject dependencies; everything is static. There is also very limited authorisation features.

The design aims to be familiar to users of Entity Framework. The Fluent API and builder pattern follows conventions found in Entity Framework Core 2.

# RestContext

It all happens in your derived `RestContext`.

```csharp
public class FootballRestContext : RestContext
{
}
```

## ApiRoot properties

You can specify just the root objects using public `ApiRoot<Team>` properties.

```csharp
public class FootballRestContext : RestContext
{
    public ApiRoot<Team> Teams { get; set; }
    public ApiRoot<Player> Players { get; set; }
}
```

That is all you need! Each root will be setup automatically. All public properties on the item type will be mapped to an API field following the naming conventions provided in the configuration.

## Builder API

You can override the `OnApiCreating` method in your derived `RestContext` and use the [`IApiBuilder`](/Fluent/Builder-API) to configure your API.

Any item types or properties used multiple times will be extended, instead of overwriting or duplicating. If `ApiRoot` properties are also used, the builder will extend any auto-configured items and fields.

```csharp
public class FootballRestContext : RestContext
{
    public ApiRoot<Team> Teams { get; set; }
    public ApiRoot<Player> Players { get; set; }

    protected override void OnApiCreating(IApiBuilder apiBuilder)
    {
        apiBuilder.Item<Team>(e =>
        {
            e.Field(t => t.FoundedDate.Year).HasName("year");
            e.Identifier(t => t.Name.Replace(" ", "").ToLower());
        });
    }
}
```

The above example will automatically configure `Team` class, then add two further customisations. A request could look something like this:

```http
GET /teams/manchestercity?fields=id,year,players

200 OK
{
    "id": 321,
    "year": 1880,
    "players": [
        { id: 234 },
        { id: 235 },
        { id: 236 },
    ]
}
```