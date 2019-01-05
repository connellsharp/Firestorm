# Configuration Builder

Firestorm hosts (such as ASP.<span />NET Core and OWIN) will provide an instance of `IFirestormServicesBuilder` that allows you to customise Firestorm.

Each Firestorm package will provide extensions to configure them independently. A typical configuration may look like this:

```csharp
services.AddFirestorm()
    .AddEndpoints(c =>
    {
        c.ResponseConfiguration.ShowDeveloperErrors = true;
    })
    .AddStems()
    .AddEntityFramework<ExampleDataContext>();
```

## AddEndpoints

All Firestorm applications will need the `Firestorm.Endpoints` package referenced and will need to call `AddEndpoints` to configure the interaction between the API request from the client and Firestorm resources.

The default configuration is used if no arguments are given. Other overloads are available to configure the `EndpointConfiguration` object.

- The `ResponseContentGenerator` gets the objects for returning to the client  in the response body, based on objects defines in `Firestorm.Core.Web`.
- The `QueryStringConfiguration` defines which keywords and operators can be used in querystrings.
- The `RequestStrategies` property contains several strategy sets defining what *unsafe* methods (e.g. POST, PUT, PATCH) should do to the different resource types.
- The `NamingConventionSwitcher` converts the client request field names to your .NET member names.

## StartResourceFactory

Using Endpoints requires you to register an implementation of `IStartResourceFactory`.

This object defines how to get the starting `IRestResource` i.e. the root directory of the API. All other resources are navigated to as ancestors of this resource.

To use [Stems](../stems/stems-intro.md), you can use the `AddStems` extension to configure a `StemsStartResourceFactory`.

To use the [Fluent API](../fluent/fluent-intro.md), use the `AddFluent` extension to configure a `FluentStartResourceFactory`.
