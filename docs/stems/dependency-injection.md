# Dependency Injection

Your `Stem` classes can also use constructor parameters for dependency injection.

Arguments will be injected into the constructor based on the `IDependencyResolver` used in the `StemConfiguration`.

One instance of each `Stem` class is created per request that uses it. A request to `/artists/123/albums/` will instantiate an `ArtistsStem` and an `AlbumsStem`.

## Constructor parameters

For example, you may way to inject a push notification service to notify subscribers when fields are set.

```csharp
public class ArtistsStem : Stem<Artist>
{
    private readonly IPushNotificationService _pushService;

    public ArtistsStem(IPushNotificationService pushService)
    {
        _pushService = pushService;
    }

    public void SetName(Artist artist, string name)
    {
        string oldName = artist.Name;
        artist.Name = name;
        _pushService.NotifyChange("name", oldName, name);
    }
}
```