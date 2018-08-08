> *Note:* Firestorm is in development.

# Firestorm

Firestorm is a REST API framework for .NET. The aim is to provide a neat and easy ways to write standardised APIs with more of the leg-work taken care of.

_Using [Stems](docs/stems/stems-intro.md) to describe your API_

```csharp
public class ArtistsStem : Stem<Artist>
{
    [Get, Identifier]
    public static int Id { get; }

    [Get, Set]
    public static string Name { get; }
}
```

_Exposes [RESTful endpoints](docs/endpoints/querying.md)_

```http
GET /artists/123
```
```json
{
    "id": 123,
    "name": "Noisia"
}
```

## Features

1. **Clean.** Lets you write neat and concise code to describe your API and exposes lightweight, human-readable responses.

2. **Powerful.** Provides querying capabilities that combines database queries and application code.

3. **Configurable.** Customise your conventions, response structure, verb strategies to suit your API needs. Integrate with your web host, ORM and IoC to fit nicely in your solution.

4. **Agile.** It's easy to start a basic project with just a few endpoints and grow rapidly as requirements build.

You can read more in the [documentation](https://firestorm.readthedocs.org), or jump straight into the [tutorials](https://github.com/connellw/Firestorm/wiki/Tutorials).

## Install

Firestorm is available from the GitHub repository and as NuGet Packages.

While it's still in early stages, packages are available from the [MyGet feed](https://www.myget.org/F/firestorm/api/v3/index.json). Be sure to use the `-prerelease` flag.

```
PM> Install-Package Firestorm.Stems.Start
PM> Install-Package Firestorm.AspNetCore2
PM> Install-Package Firestorm.EntityFrameworkCore2
```

See the [Installation](docs/setup/installation.md) section for more detailed setup information.

## About

Firestorm is a bit of an experiment that grew into something I feel other developers could use. It was never a clearly defined project, but I decided to set some milestones and make it my first open-source project.

It's still in active development. I have so many feature ideas for future versions!

#### Copyright
Copyright � 2018 Connell Watkins

#### License
Firestorm is licensed under MIT. Refer to [LICENSE.txt](LICENSE.txt) for detailed information.