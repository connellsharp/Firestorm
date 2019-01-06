# Authorization

Firestorm provides basic authorization at a field, item and collection levels.

!!!note
	v1.0 provides basic authorization. A more comprehensive set of authorisation features is planned for a future version.

## AuthorizeAttribute for fields

The `Authorize` attribute determines whether the API user can access the field.

The attribute can allow specific Users or Roles, as with ASP.NET MVC's `AuthorizeAttribute`.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Get]
    public static Expression<Func<Artist, string>> Name
    {
        get { return a => a.Name; }
    }

    [Set] 
    [Authorize(Roles = "Admin")]
    public void SetName(Artist artist, string name)
    {
        artist.OldName = artist.Name;
        artist.Name = name;
        artist.NameChangedDate = DateTime.Now;
    }
}
```

``` json
PUT /artists/123/name
"Noisia"

401 Unauthorized                
{
    "error": "authorization",
    "message": "You do not have permissions to set the 'name' field."
}
```

## PermissionExpression for items

The `Stem<>` class declares the virtual property `Expression<Func<TItem, ItemPermission>> PermissionExpression` for you to override with item-level permissions.

The `ItemPermission` enum returned by this expression determines what actions the user can do for the given item.

```csharp
public enum ItemPermission
{
	None = 0,
	Read = 1,
	Write = 2,
	ReadWrite = Write | Read,
	Delete = 4,
	ReadWriteDelete = Delete | ReadWrite,
}
```

For example, you can make an expression that allows the user to view all artists, but only edit their own.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    public override Expression<Func<Artist, ItemPermission>> PermissionExpression
    {
        get { return a => a.OwnedByUsername == User.Username ? ItemPermission.ReadWrite : ItemPermission.Read; }
    }
}
```

This is particularly handy if you want to limit a collection to only return certain items. The expression is used in the `Where` clause in the final LINQ query.

This method defaults to return `null`, where no filter will be added to the final query and all users are assumed to have full `ReadWriteDelete` permissions.

## CanAddItem for collections

Finally, there's another virtual method, `bool CanAddItem()`.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    public override bool CanAddItem()
    {
        return User.IsInRole("Create_Artists");
    }
}
```

This method defaults to always return `true`, allowing any user to add new items to the collection.