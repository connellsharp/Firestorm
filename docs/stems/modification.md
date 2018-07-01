# Modification

The `Stem` base class contains some virtual methods that are called at certain stages in a request.

## OnSaving and OnSaved

Saving changes happens in the background with a single `SaveChanges()` call to your repository. Stems provide ways to execute custom code before and after that call.

The `OnSavingAsync` method is executed before this call. Any exceptions thrown in that method will be returned to the client and the entity is not saved.

The `OnSavedAsync` method is executed **after** the changes have been committed to the repository. Any modifications to the entity object made here won't be saved.

```c#
public class ArtistsStem : Stem<Artist>
{
    // Contructor and properties omitted for brevity.

    protected override async Task OnSavingAsync(Artist artist)
    {
        artist.LastModified = DateTime.UtcNow;
    }

    protected override async Task OnSavedAsync(Artist artist)
    {
        await _eventPublisher.PublishAsync(new ArtistModifiedEvent
        {
            ArtistId = artist.Id
        });
    }
}
```

## Create

The `Create` method is called to instantiate a new entity. By default, this is called when a client performs a `POST` request to the collection.

```c#
public class ArtistsStem : Stem<Artist>
{
    // Contructor and properties omitted for brevity.

    protected override Artist Create()
    {
        return new Artist
        {
            CreatedDate = DateTime.UtcNow,
            CreatedByUserName = User.Identity.Name
        };
    }
}
```

## MarkDeleted

The `MarkDeleted` method is called when the entity is deleted. As with `Create`, the deletion is not committed to the repository until `OnSavedAsync` is called.

Overriding this method allows you to perform a "soft delete" where the entity remains in the database. To ensure these entities don't appear in your API queries after being marked as deleted, you can override `IsVisibleExpression`.

```c#
public class ArtistsStem : Stem<Artist>
{
    // Contructor and properties omitted for brevity.

    protected override Task MarkDeleted(Artist artist)
    {
        artist.IsDeleted = true;
    }

    protected override Expression<Func<Artist, bool>> IsVisibleExpression
    {
        get { return a => !a.IsDeleted; }
    }
}
```