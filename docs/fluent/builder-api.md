# Builder API

You can override the `OnApiCreating` method in your derived `RestContext` and use the `IApiBuilder` to configure your API.

```csharp
public class FootballRestContext : RestContext
{
    protected override void OnApiCreating(IApiBuilder apiBuilder)
    {
        // Use apiBuilder here
    }
}
```

The starting point for root items is the `.Item<T>()` method, which returns a builder.

## AutoConfigure

Items can be automatically configured. This pulls all public properties and uses the `NamingConventionSwitcher` to determine the field's key in your API.

```csharp
    protected override void OnApiCreating(IApiBuilder apiBuilder)
    {
        apiBuilder.Item<Team>().AutoConfigure();

        apiBuilder.Item<Player>().AutoConfigure();
    }
```

## Configuring manually

Or you can configure the items manually.

```csharp
public class FootballRestContext : RestContext
{
    protected override void OnApiCreating(IApiBuilder apiBuilder)
    {
        apiBuilder.Item<Team>(e =>
        {
            e.RootName = "teams";

            e.Field(t => t.Name);

            e.Field(t => t.FoundedYear)
                .HasName("founded");

            e.Field(t => t.Players)
                .IsCollection(p => {
                    p.Field(p => p.Name);
                    p.Field(p => p.SquadNumber);
                });
        });

        apiBuilder.Item<Player>(e =>
        {
            e.RootName = "players";

            e.Field(p => p.Name)
                .AllowWrite();

            e.Field(p => p.Team)
                .IsItem();
        });
    }
}
```

You can also extend the auto configured items this way.