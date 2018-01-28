# Configuration Object

Firestorm starting endpoints will all accept a `FirestormConfiguration` object in the main startup method, for example as a parameter in `app.UseFirestorm` in ASP.NET Core and OWIN.

This object contains hierarchical configuration data and components used by various parts of the Firestorm architecture.

A typical configuration may look like this:

```csharp
new FirestormConfiguration
{
    EndpointConfiguration = new RestEndpointConfiguration
    {
        ResponseContentGenerator = new StatusCodeResponseContentGenerator(),
        QueryStringConfiguration = new CollectionQueryStringConfiguration(),
        RequestStrategies = new UnsafeRequestStrategySets(),
        NamingConventionSwitcher = new DefaultNamingConventionSwitcher()
    },
    StartResourceFactory = new StemsStartResourceFactory
    {
        // Stems configuration ommitted
    }
}
```

## EndpointConfiguration

- The `EndpointConfiguration` object configures the interaction between the API request from the client and Firestorm resources.

    - The `ResponseContentGenerator` gets the objects for returning to the client  in the response body, based on objects defines in `Firestorm.Core.Web`.
    - The `QueryStringConfiguration` defines which keywords and operators can be used in querystrings.
    - The `RequestStrategies` property contains several strategy sets defining what *unsafe* methods (e.g. POST, PUT, PATCH) should do to the different resource types.
    - The `NamingConventionSwitcher` converts the client request field names to .NET Stem member names.

## StartResourceFactory 

- The `IStartResourceFactory` implementation defines how to get the starting `IRestResource` i.e. the root directory of the API. All other resources are navigated to as ancestors of this resource.

