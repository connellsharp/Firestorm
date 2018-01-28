# Install Nuget Package

```
PM> Install-Package Firestorm.Stems
```

That's all you need to start writing **[Stems](/Tutorials/Stems)**! For good separation, you can keep your Stem classes in their own assembly.

But those stems only describe what users can do with the data. They need something to feed them the information. They need **[Roots](/Tutorials/Roots)**, and there are a few ways of doing this.

Whichever way you choose, all the Root functionality will be accessible to the Firestorm Endpoints via an implementation of `IStartResourceFactory`.

# Web Startup

For all supported web API technologies, a NuGet package is provided that depends on `Firestorm.Endpoints.Start`. Here in lies the main [Configuration](/Tutorials/Configuration), the `FirestormConfiguration` class, containing all your API settings.

To use Stems, you must set the `StartResourceFactory` property to `StemsStartResourceFactory`.

In the following examples we will configure our application to use a `DataSourceRootResourceFactory` with Entity Framework. To avoid duplication, we will put our config into a static class.

```csharp
internal static class FirestormStartup
{
    internal static FirestormConfiguration Configuration = new FirestormConfiguration
    {
        StartResourceFactory = new StemsStartResourceFactory
        {
            RootFactory = new DataSourceRootResourceFactory
            {
                StemsNamespace = "YourApplication.Stems",
                DataSource = new EntitiesDataSource<YourApplication.Data.EntitiesContext>()
            }
        }
    };
}
```

Now we can use that config in our web application's startup.

## ASP<span>.</span>NET Core

Firestorm provides Middleware for ASP<span>.</span>NET Core and a `UseFirestorm` extension method.

```
PM> Install-Package Firestorm.Endpoints.AspNetCore
```

```csharp
public class Startup
{
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseFirestorm(FirestormStartup.Configuration);
    }
}
```

## OWIN

Similarly, another package provides a different `UseFirestorm` extension method to setup OWIN Middleware.

```
PM> Install-Package Firestorm.Endpoints.Owin
```

```csharp
public class Startup
{
    public void Configure(IAppBuilder app)
    {
        app.UseFirestorm(FirestormStartup.Configuration);
    }
}
```

## WebAPI 2.0

ASP<span>.</span>NET Web API 2.0 is also supported through a `FirestormController`.

You can apply the default route mapping with the `SetupFirestorm` extension method.

```
PM> Install-Package Firestorm.Endpoints.WebApi2
```

```csharp
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        config.SetupFirestorm(FirestormStartup.Configuration);
    }
}
```