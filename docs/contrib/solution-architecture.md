# Solution architecture

## Core
At the heart of Firestorm is a small set of interfaces and classes defining the types of resources and what you can do with them.

- **Scalars** are single values i.e. strings, numbers, dates and bools.
- **Items** are a set of unique field names that describe an object mapped to another resource value.
- **Collections** contain several items that can be identified, queried and grouped.
- **Dictionaries** are collections that have been grouped by a specific field.
- **Directories** are sets of named collections, usually used for the root resource.

## Core Web
Just outside that, another set of classes set-out a common way to describe resource data and actions that are transmitted.

- Resource Body
- Feedback
    - Acknowledgement says the request has been fulfilled.
    - Error states someone went wrong when processing the request.
    - Multi Response is an array of Feedback responses

## Data

Simply defining a common Repository and Unit of Work pattern.

- Repository
- Transaction
- Data Source

## Endpoints
Endpoints are **consumers** of Firestorm Core that expose a RESTful API.

![image.png](.attachments/image-51e948ef-b613-4ed5-98ba-f14964c6a91c.png)

Resources are queried and updated using implementations of `IRestEndpoint`, e.g. `RestCollectionEndpoint`, `RestItemEndpoint`, `RestScalarEndpoint`.

`IRestEndpoint` defines a `Next` method, allowing the endpoints to be chained together.

    Root.Next("artists").Next("123").Next("name").GetAsync();

#### Formatting
Serialises and deserialises resource bodies into common web data formats.

- JSON
- XML
- YAML
- URL Encoded

#### Start

Defines an `IStartResourceFactory` interface to get the first resource in the chain.

There are several implementations for different web frameworks:
- ASP.NET Core
- OWIN
- Web API

## Engine

The Engine is an **implementation** of Firestorm Core that uses Expression Trees to build queries for an `IQueryable`. It makes use of deferred execution.

![image.png](.attachments/image-e5348081-860c-4e58-8417-9d99a355aaf1.png)

Defines an `IEngineContext` to be used in Engine resources. The context contains interfaces that describe how to:

- Manage the data transaction lifetime.
- Query and add to the data repository.
- Identify items within a collection.
- Read and write fields.
- Check the request is authorised.

#### Additives
Provides logic to create common field readers and writers from `PropertyInfo`, `LambdaExpression`, delegates and others.

#### Implementation
The **implementation** of Firestorm Core resources. Resources are implemented by passing an `IEngineContext` instance around as the resources are navigated.

`RestEngineCollection<>`, `RestEngineItem<>`, `RestEngineScalar<>` all require the `IEngineContext` instance as a constructor parameter.

#### Subs
Extending the implementation to include navigation properties. Several `IEngineContext` implementations for different types of entity relationships combine to build nested queries, essentially using `.Select()` and `.SelectMany()`.

## Entities

A few entity libraries are available to integrate with your ORM. These simply wrap the framework and implement interfaces defined in `Firestorm.Data`.

## Stems
Stems provides ways to decorate classes to describe your REST API.

![image.png](.attachments/image-d5d955fa-3bac-4c4d-828d-c6b64bd3910b.png)

Each `Stem<>` class handles one item type (for example a DTO or auto-generated Entity class) and maps API behaviours to its members.

#### Attributes
Defines the attributes used to decorate Stem classes and defines ways to analyse them to build Definitions.

- **Basic**: Currently all the attributes. `Get`, `Set`, `Identifier`, `Authorize`, etc.

#### Fuel
Contains **implementations** of the interfaces used by the Engine, providing a `StemEngineContext` that uses a cached analysis of the Definitions.

Cache is built using factories and resolvers from the `IStemsFeatureSet` objects in the `StemConfiguration`. Feature sets are defined in derived libraries. Additional feature sets can be installed separately.

- **Essential**: builds the expressions and delegates from the main `Get`, `Set` attributes.
- **Substems**: navigate though sub-collections and sub-items defined with the `Substem` attribute.

#### Roots
The Roots library provides an `IRootResourceFactory` interface that give the root directory.

There are two main implementations:
- **Derive**: Create `Root<>` classes that handle the same type of item as the `Stem<>` class.
- **DataSource**: Pass in an implementation of `IDataSource` and decorate Stems with the `DataSourceRoot` attribute.

And the top library that ties it all together:
- **Start**: Defines the `StemsStartResourceFactory` to tie this all into the `Endpoints.Start`.

## Fluent

The Fluent API is a simpler alternative to Stems. It is not as feature-rich, but more compact and sometimes more familiar.

![image.png](.attachments/image-6e05311f-1627-415f-880b-5681dda10473.png)

#### Fuel
Much like `Stems`, contains **implementations** of the interfaces used by the Engine, providing a `FluentEngineContext`.

#### Start
Defines the `FluentStartResourceFactory` to tie this all into the `Endpoints.Start`.

# Overall

Grouped together, this gives us an overall solution architecture looking like this:

![image.png](.attachments/image-f144b972-1a72-456c-8075-f90b41bd731d.png)

And it's not particularly helpful, but it's kinda cool to look at, so here's everything in one giant diagram:

![image.png](.attachments/image-ea03cbc8-dc41-449b-9e37-3d4f439c019b.png)