There are currently two ways to provide data for Stems.

# Derive

You can create `Root` classes for each `Stem` you want to expose as a starting resource.

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

Types that derive from `Root<>` are discovered and their properties are used to drive the data.

Your Roots will use the same IoC container as your Stems, so you can use constructor parameters to inject your dependencies.

Firestorm will set the `User` and `Configuration` properties straight after constructing your root class. These will be passed down into your Stems.

### Installation

To use Derived roots, reference the `Firestorm.Stems.Roots` package.

```
PM> Install-Package Firestorm.Stems.Roots
```

Similarly to Stems, types are discovered automatically and overloads are provided to specify the assembly and namespace to scan.

```csharp
services.AddFirestorm()
    .AddStems()
    .AddRoots();
```

# Data Source

Often, the **Derive** methodology produces a lot of very similar Root classes. The alternative is to use a single `IDataSource` implementation to drive all your Roots.

### IDataSource

The `IDataSource` implementation is responsible for creating new data transactions and creating repositories for given data types.

You can register a single `IDataSource` instance using the `AddDataSource` extension.

```csharp
services.AddFirestorm()
    .AddStems()
    .AddDataSource(new MyDataSource());
```

With this methodology, your Stems don't have a parent `Root` to pass down the `User` and `Configuration` properties. Instead, the package creates a fake object for your Stems to start from called a `Vase`.

### NoDataSourceRootAttribute

By default, all Stems can be used as Roots.

You can decorate those you don't want to use as Roots with the `NoDataSourceRoot` attribute.

```csharp
[NoDataSourceRoot]
public class AuthorsStem : Stem<Author>
{
}
```

## Entity Framework

You can use off-the-shelf `IDataSource` implementations. If your Stems describe usage of Entity Framework models directly, you can use the `AddEntityFramework<>` extension with your `DbContext` type.

```
PM> Install-Package Firestorm.EntityFramework
```

```csharp
services.AddFirestorm()
    .AddStems()
    .AddEntityFramework<MyDbContext>();
```