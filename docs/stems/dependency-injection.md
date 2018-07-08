# Dependency Injection

Your `Stem` classes can also use constructor parameters for dependency injection.

Arguments will be injected into the constructor based on the `IDependencyResolver` used in the `StemConfiguration`.

One instance of each `Stem` class is created per request that uses it. A request to `/artists/123/albums/` will instantiate an `ArtistsStem` and an `AlbumsStem`.

## Constructor parameters

For example, you may want to inject an event publisher to notify subscribers when fields are set.

```csharp
public class ArtistsStem : Stem<Artist>
{
    private readonly IEventPublisher _eventPublisher;

    public ArtistsStem(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    [Set]
    public void SetName(Artist artist, string name)
    {
        string oldName = artist.Name;
        artist.Name = name;

        _eventPublisher.Publish(new ArtistNameChangingEvent 
        {
            ArtistId = artist.Id,
            OldName = oldName,
            NewName = name
        });
    }
}
```

It is worth noting that the changes have not been saved to the database at this point. It would be better practice to queue the event in the Stem instance and raise it in the [`OnSaved` method](events.md).