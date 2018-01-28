# ASP<span/>.NET Core

Firestorm provides Middleware for ASP<span>.</span>NET Core and a `UseFirestorm` extension method.

```
PM> Install-Package Firestorm.AspNetCore
```

## Full Configuration Object

If you like the consistency with, or are just migrating from another web framework, you might like to pass the whole configuration object into the `UseFirestorm` method in your `Configure` method.

```csharp
public class Startup
{
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseFirestorm(new FirestormConfiguration
        {
            // Config omitted for brevity
        });
    }
}
```

But that doesn't feel very Core-y...

## ConfigureServices

You can configure Firestorm using extensions in the `ConfigureServices` method of your `Startup` class.

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFirestorm()
            .ConfigureEndpoints(config =>
            {
                config.QueryStringConfiguration.SpecialFilterKeysEnabled = false;
                // Config omitted for brevity
            })
            .AddStartResourceFactory(new StemsStartResourceFactory()
            {
                // Config omitted for brevity
            });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseFirestorm();
    }
}
```

## Half and half

You can configure just the `IStartResourceFactory` in `ConfigureServices` and the API endpoint stuff in `Configure`. This might just feel right.

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFirestorm()
            .AddStartResourceFactory((new StemsStartResourceFactory
            {
                // Config omitted for brevity
            });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseFirestorm(new RestEndpointConfiguration
        {
            // Config omitted for brevity
        });
    }
}
```

## Extensions

You can also use extensions from the `Firestorm.Extensions.AspNetCore` NuGet package to configure the start resource factory.

The package includes extensions for [Stems](Stems), [Fluent](Fluent) and [Entity Framework](EntityFramework).

```
PM> Install-Package Firestorm.Extensions.AspNetCore
```

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFirestorm()
            .AddStems()
            .AddEntityFramework<YourDbContext>();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseFirestorm();
    }
}
```