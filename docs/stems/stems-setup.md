# Install Nuget Package

```
PM> Install-Package Firestorm.Stems
```

That's all you need to start writing Stem classes! For good separation, you can keep your Stem classes in their own assembly.

# Configuring Stems

In your [configuration](../setup/configuration-builder.md) builder, use the `AddStems` extension.

```csharp
services.AddFirestorm()
	.AddStems()
```



### StemConfiguration

The factory requires a `StemConfiguration` object, which contains your stem configuration. This contains further settings related to Stems.

- The **NamingConventionSwitcher** switches the request/response naming conventions with your Stem members'.
- The **AutoPropertyMapper** automatically gets `Expression`s from a [simpler property signature](expression-syntax-options.md#auto-mapping).
- The **DependencyResolver** provides the services used for [dependency injection](dependency-injection.md).

### RootResourceFactory 

The `StartResourceFactory` requires a `RootResourceFactory ` to feed the Stems with the data they need. There are a couple of different ways to define **[Roots](roots.md)**.

In these examples, we will use the `DataSourceRootFactory` to use Entity Framework Core for the Root data.

!!!note
    There are plans to redesign Roots as a separate `IDataSource` implementation, meaning there would be no need for this extra `RootResourceFactory`.

## Web Startup

Now you can use your configuration object in your web startup.

### ASP<span>.</span>NET Core 2.0

!!!note
	This example uses ASP<span>.</span>NET Core but you can use the same config [with other web technologies](../setup/installation.md).

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
			StartResourceFactory = new StemsStartResourceFactory
			{
				RootFactory = new DataSourceRootFactory
				{
					StemsNamespace = "YourApplication.Stems",
					DataSource = new EntitiesDataSource<YourApplication.Data.EntitiesContext>()
				}
			}
		});
    }
}
```

### ASP<span>.</span>NET Core Extensions

Alternatively you can use the extensions.

```
PM> Install-Package Firestorm.Extensions.AspNetCore2
```

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFirestorm()
			.AddStems("YourApplication.Stems")
			.AddEntityFramework<YourApplication.Data.EntitiesContext>();
    }
	
    public void Configure(IApplicationBuilder app)
    {
        app.UseFirestorm();
    }
}
```