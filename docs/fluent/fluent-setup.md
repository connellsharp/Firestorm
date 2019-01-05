# Install Nuget Package

```
PM> Install-Package Firestorm.Fluent
```

That's all you need to start using the Fluent API!

You could put your fluent API file in a whole library to itself if you want.

# Configuration

To use the Fluent API, you must use the `AddFluent` extension for your [configuration builder](../setup/configuration-builder.md) and register your implementation of `IApiContext`.

There are several overloads to register your context by type or by instance. The default parameterless overload will use the context instance registered in the container.

```csharp
services.AddFirestorm()
    .AddFluent();
```

# No Context

For simple APIs, it might be overkill to create a context class, so an overload is provided to allow direct configuration using the `IApiBuilder`.

```csharp
services.AddFirestorm()
    .AddFluent(builder => {
        builder.Item<Team>(e =>
        {
            e.Identifier(t => t.Id);
            e.Field(t => t.Name);
        });
    });
```

# Auto Configuration

The Fluent API can also be used to automatically detect and configure your API. An overload is provided that accepts an `AutoConfiguration` object.

```csharp
services.AddFirestorm()
    .AddFluent(new AutoConfiguration
    {
        AllowWrite = true,
        MaxNesting = 4
    });
```