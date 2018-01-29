# Install Nuget Package

```
PM> Install-Package Firestorm.Fluent
```

That's all you need to start using the Fluent API!

You could put your fluent API file in a whole library to itself if you want.

# Web Startup

To use the Fluent API, you must set the `EndpointConfiguration.StartResourceFactory` property to `FluentStartResourceFactory`.

```csharp
internal static class FirestormStartup
{
    internal static FirestormConfiguration Configuration = new FirestormConfiguration
    {
        StartResourceFactory = new FluentStartResourceFactory
        {
			Context = new YourApplication.RestContext(),
			DataSource = new EntitiesDataSource<YourApplication.EntitiesContext>()
        }
    };
}
```

Now we can use that config in our [web application's startup](../setup/installation.md).

## ASP<span>.</span>NET Core 2.0

```
PM> Install-Package Firestorm.AspNetCore2
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

#### Extensions

Alternatively you can use the extensions.

```
PM> Install-Package Firestorm.AspNetCore2
```

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFirestorm()
			.AddFluent();
    }
	
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseFirestorm();
    }
}
```



## OWIN

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

### WebAPI 2.0

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