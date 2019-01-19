# NuGet Packages

Due to the modular design of Firestorm, there are many NuGet packages available to meet your needs. You won't need all of them.

See the [Installation tutorial](/Tutorial/Installation) for more of a step-by-step guide.

##  Core

The eye of the storm.

The `Firestorm.Core` package contains the core abstractions used by other Firestorm packages.

## Endpoints

`Firestorm.Endpoints` translates HTTP requests into calls to the Firestorm Core abstractions.

It brings together sub-packages such as `Formatting` and `Responses` and allows configuring your APIs default behaviour.

## Host

`Firestorm.Host` is the base package for adapting web hosting technologies to use Firestorm Endpoints.

It is rarely used on its own, unless you're building support for a new Web API Framework. This is used by the `AspNetCore2`, `Owin` and `AspNetWebApi2`packages.

### ASP<span>.</span>NET Core 2

ASP<span>.</span>NET Core middleware and helpers.

```nuget
Install-Package Firestorm.AspNetCore2
```

### OWIN

OWIN middleware and helpers

```nuget
Install-Package Firestorm.Owin
```

### Web API 2.0

ASP.NET Web API 2 controller and helpers

```nuget
Install-Package Firestorm.AspNetWebApi2
```

## Engine

The Firestorm Engine builds the `IQueryable` objects that are executed by your ORM.

The Engine itself isn't designed to be used directly by your application. It is used by `Stems` and `Fluent` packages.

## Data

The `Firestorm.Data` package used by the Engine defined abstractions for your ORM or persistence framework. There are several possible implementations you could use.

### Entity Framework 6

Use Entity Framework as a data source.

```nuget
Install-Package Firestorm.EntityFramework6
```

### Entity Framework Core 2

Use Entity Framework Core as a data source.

```nuget
Install-Package Firestorm.EntityFrameworkCore2
```

## Stems

The `Firestorm.Stems` package installs all sub-packages required to use [Stems](../stems/stems-intro.md) in your application.

```nuget
Install-Package Firestorm.Stems
```

### Stems.Essentials

The `Firestorm.Stems.Essentials` package only installs the basic features for Stems, without including `Roots` or any hosting extensions.

This package could be used if your `Stem` classes are defined in a sub-library designed to be referenced by your main host library.

```nuget
Install-Package Firestorm.Stems.Essentials
```

## Fluent

The `Firestorm.Fluent` package installs all sub-packages required to use the [Fluent API](../fluent/fluent-intro.md) in your application.

```nuget
Install-Package Firestorm.Fluent
```

### Fluent.Fuel

As with Stems, the `.Fuel` package also only installs basic features.

This package could be used if your `ApiContext` is defined in a sub-library.

```nuget
Install-Package Firestorm.Fluent.Fuel
```

## Metapackage

For convinience, the package for the root namespace `Firestorm` is a metapackage that contains a common setup of:

- Endpoints
- ASP.NET Core 2.0
- Entity Framework Core 2.0
- Stems
- Fluent API

```nuget
Install-Package Firestorm
```