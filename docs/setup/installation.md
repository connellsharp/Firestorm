# Web Startup

For all supported web API technologies, a NuGet package is provided that depends on `Firestorm.Endpoints`. Here in lies the main [configuration object](configuration-object.md), the `FirestormConfiguration` class, containing all your API settings.

## ASP<span>.</span>NET Core

Firestorm provides Middleware for ASP<span>.</span>NET Core and a `UseFirestorm` extension method.

```
PM> Install-Package Firestorm.AspNetCore2
```

```csharp
public class Startup
{	
    public void Configure(IApplicationBuilder app)
    {
        app.UseFirestorm(new FirestormConfiguration
		{
			// Configuration omitted for brevity
		});
    }
}
```

Typically, in a new project, it's recommended to use the [extensions](aspnetcore-startup.md).

```
PM> Install-Package Firestorm.Extensions.AspNetCore
```

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFirestorm();
			// Services omitted for brevity
    }
	
    public void Configure(IApplicationBuilder app)
    {
        app.UseFirestorm();
    }
}
```

## OWIN

Another package provides a different `UseFirestorm` extension method to setup OWIN Middleware.

```
PM> Install-Package Firestorm.Owin
```

```csharp
public class Startup
{
    public void Configure(IAppBuilder app)
    {
        app.UseFirestorm(new FirestormConfiguration
		{
			// Configuration omitted for brevity
		});
    }
}
```

## WebAPI 2.0

ASP<span>.</span>NET Web API 2.0 is also supported through a `FirestormController`.

You can apply the default route mapping with the `SetupFirestorm` extension method.

```
PM> Install-Package Firestorm.AspNetWebApi2
```

```csharp
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        config.SetupFirestorm(new FirestormConfiguration
		{
			// Configuration omitted for brevity
		});
    }
}
```