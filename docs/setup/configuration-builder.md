# Configuration Builder

Firestorm hosts (such as ASP.<span />NET Core and OWIN) will provide an instance of `IFirestormServicesBuilder` that allows you to customise Firestorm.

Each Firestorm package will provide extensions to configure them independently. A typical configuration may look like this:

```csharp
services.AddFirestorm()
    .AddEndpoints(c =>
    {
        c.Response.ShowDeveloperErrors = true;
    })
    .AddStems()
    .AddEntityFramework<ExampleDataContext>();
```

## AddEndpoints

All Firestorm applications will need the `Firestorm.Endpoints` package referenced and will need to call `AddEndpoints` to configure the interaction between the API request from the client and Firestorm resources.

The default configuration is used if no arguments are given. Other overloads are available to configure the [`EndpointConfiguration` object](../endpoints/endpoint-config.md).

## StartResourceFactory

Using Endpoints requires you to register an implementation of `IStartResourceFactory`.

This object defines how to get the starting `IRestResource` i.e. the root directory of the API. All other resources are navigated to as ancestors of this resource.

To use [Stems](../stems/stems-intro.md), you can use the `AddStems` extension to configure a `StemsStartResourceFactory`.

To use the [Fluent API](../fluent/fluent-intro.md), use the `AddFluent` extension to configure a `FluentStartResourceFactory`.
