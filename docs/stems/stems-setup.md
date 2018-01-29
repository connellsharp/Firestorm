# Install Nuget Package

```
PM> Install-Package Firestorm.Stems
```

That's all you need to start writing Stems! For good separation, you can keep your Stem classes in their own assembly.

But those stems only describe what users can do with the data. They need something to feed them the information. They need **[Roots](roots.md)**, and there are a few ways of doing this.

In these examples, we will use the `DataSourceRootFactory` to use Entity Framework Core for the Root data.

To use Stems, you must set the `EndpointConfiguration.StartResourceFactory` property to `StemsStartResourceFactory`.

## ASP<span>.</span>NET Core 2.0

_Note: This example uses ASP<span>.</span>NET Core but you can use the same config [with other web technologies](../setup/installation.md)._

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

## ASP<span>.</span>NET Core Extensions

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
			.AddStems("YourApplication.Stems")
			.AddEntityFramework<YourApplication.Data.EntitiesContext>();
    }
	
    public void Configure(IApplicationBuilder app)
    {
        app.UseFirestorm();
    }
}
```