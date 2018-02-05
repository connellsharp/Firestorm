There are currently two ways to provide data for Stems.

Whichever way you choose, you will be given an implementation of `IRootResourceFactory`, which is given to the `StemsStartResourceFactory` (see [Setup](stems-setup.md)).

!!!note
    There are plans to redesign Roots as a separate `IDataSource` implementation, meaning there would be no need for the extra `RootResourceFactory`.

# Derive

You can create `Root` classes for each `Stem` you want to expose as a starting resource.

```
PM> Install-Package Firestorm.Stems.Roots.Derive
```

Like our familiar friend, the `Stem<>`, you can derive from `Root<>` to create Root classes.

As with stems, an instance of the `Root<>` class is created once per API request.

```csharp
public class AuthorsRoot : Root<Author>
{
    public override IQueryable<Author> GetAllItems()
    {
        // return your root queryable here.
    }
}
```

The factory for this is the `RootsStartResourceFactory`.

Types that derive from `Root<>` are discovered and their properties are used to drive the data. Types can be discovered automatically by their Namespace, and/or specific types can be passed in.

```csharp
StartResourceFactory = new RootsStartResourceFactory
    {
        RootTypes = new { typeof(AuthorsRoot), typeof(BooksRoot) },
        RootNamespace = "YourApplication.Roots"
    }
```

Your Roots will use the same IoC container as your Stems, so you can use constructor parameters to inject your dependencies.

Firestorm will set the `User` and `Configuration` properties straight after constructing your root class. These will be passed down into your Stems.

# Data Source

Often, the **Derive** methodology produces a lot of very similar Root classes. The alternative is to use a single `IDataSource` implementation to drive all your Roots.

```
PM> Install-Package Firestorm.Stems.Roots.DataSource
```

### IDataSource

The `IDataSource` implementation is responsible for creating new data transactions and creating repositories for given data types.

You feed a single `IDataSource` into a `DataSourceStartResourceFactory`.

```csharp
StartResourceFactory = new DataSourceStartResourceFactory
{
	DataSource = YourDataSourceImplementation(),
	StemTypes = new { typeof(AuthorsStem), typeof(BooksStem) },
	StemsNamespace = "YourApplication.Stems"
}
```

With this methodology, your Stems don't have a parent `Root` to pass down the `User` and `Configuration` properties. Instead, the package creates a fake object for your Stems to start from called a `Vase`.

### DataSourceRootAttribute

Not all stems will be used as Roots. You will need to decorate those you want to use as Roots with the `DataSourceRoot` attribute.

```csharp
[DataSourceRoot]
public class AuthorsStem : Stem<Author>
{
}
```

## Entity Framework

You can use off-the-shelf `IDataSource` implementations. If your Stems describe usage of Entity Framework models directly, you can use the `EntityFrameworkDataSource<>` with your `DbContext` type.

```
PM> Install-Package Firestorm.EntityFramework
```

```csharp
StartResourceFactory = new DataSourceStartResourceFactory
{
	DataSource = EntityFrameworkDataSource<YourEntityContext>(),
}
```
