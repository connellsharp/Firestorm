# NuGet Packages

Due to the modular design of Firestorm, there are many NuGet packages available to meet your needs. You won't need all of them.

However, to try and simplify installation, it's not separated completely as one package [per assembly](/Documentation/Solution-architecture). Some packages group together mutliple projects that are almost always used together.

See the [Installation tutorial](/Tutorial/Installation) for more of a step-by-step guide.

##  Core

The eye of the storm.

The `Firestorm.Core` package contains the `Firestorm.Core` and `Firestorm.Core.Web` libraries.

This is rarely used on its own, but is a dependency used by all the other packages.

## Endpoints

`Firestorm.Endpoints` is the base package for Firestorm Endpoint technologies. It also contains the `Formatting` and `Start` libraries.

Again, rarely used on its own, unless you're building support for a new Web API Framework. This is used by the `AspNetCore`, `Owin` and `AspNetWebApi`packages.

### ASP<span>.</span>NET Core

ASP<span>.</span>NET Core middleware and helpers.

```nuget
Install-Package Firestorm.AspNetCore -prerelease
```

### OWIN

OWIN middleware and helpers

```nuget
Install-Package Firestorm.Owin -prerelease
```

### Web API 2.0

ASP.NET Web API 2 controller and helpers

```nuget
Install-Package Firestorm.AspNetWebApi2 -prerelease
```

## Data

The Firestorm Engine isn't available as a package on it's own. It may be at a later date.

The `Firestorm.Data` package used by the Engine is available though, and is used by two data technologies that you'd reference depending on your preference.

### Entity Framework 6

Use Entity Framework as a data source.

```nuget
Install-Package Firestorm.EntityFramework6 -prerelease
```

### Entity Framework Core 2

Use Entity Framework Core as a data source.

```nuget
Install-Package Firestorm.EntityFrameworkCore2 -prerelease
```

## Stems

The `Firestorm.Stems` package installs `Firestorm.Stems` along with the sub `Attributes` and `Attributes.Basic`.

On its own, this package can be used in a sub-library containing only your `Stem` classes.

```nuget
Install-Package Firestorm.Stems -prerelease
```

### Stems.All

The `Firestorm.Stems.All` package depends on `Firestorm.Stems`, but also adds `Roots.DataSource`, `Roots.Derive` and `Roots.Start`, all of which are also available as separate packages.

The `.All` package just provides a nice-and-easy facade to install in your application.

```nuget
Install-Package Firestorm.Stems.All -prerelease
```

## Fluent

The `Firestorm.Fluent` package installs only the `Firestorm.Fluent` library. This could be used if your `RestContext` was in a different project.

```nuget
Install-Package Firestorm.Fluent -prerelease
```

### Fluent.All

As with Stems, the `.All` package also installs `Firestorm.Fluent.Fuel` and `Firestorm.Fluent.Start`. This would usually be referenced in your application.

```nuget
Install-Package Firestorm.Fluent.All -prerelease
```