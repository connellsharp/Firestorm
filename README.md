# Firestorm

[![Build status](https://ci.appveyor.com/api/projects/status/1bo4yw50e7m7m2cm?svg=true)](https://ci.appveyor.com/project/connellw/firestorm) [![codecov](https://codecov.io/gh/connellw/Firestorm/branch/master/graph/badge.svg)](https://codecov.io/gh/connellw/Firestorm) [![MyGet](https://img.shields.io/myget/firestorm/vpre/Firestorm.Endpoints.svg?label=myget)](https://myget.org/gallery/firestorm) [![NuGet](https://img.shields.io/nuget/v/Firestorm.Endpoints.svg)](https://www.nuget.org/packages?q=firestorm)

Firestorm is a REST API framework for .NET. The aim is to provide a neat and easy ways to write standardised APIs with more of the leg-work taken care of.

_Using [Stems](docs/stems/stems-intro.md) to describe your API_

```c#
public class CharactersStem : Stem<Character>
{
    [Get, Identifier]
    public static Expression Id => Expression(p => p.Id);

    [Get]
    public static Expression Name => Expression(p => p.FirstName + " " + p.LastName);
}
```

_Exposes [RESTful endpoints](docs/endpoints/navigation.md)_

```http
GET /characters/123

{
    "id": 123,
    "name": "Eddard Stark"
}
```

## Features

1. **Clean.** Lets you write neat and concise code to describe your API and exposes lightweight, human-readable responses.

    - Natural [URL paths](docs/endpoints/navigation.md) e.g. `/characters/123/birthplace/name`.
    - To-the-point [querystrings](docs/endpoints/querying.md) e.g. `?status=alive&sort=name+asc&page=2`.
    - Uncluttered responses without extra metadata.

2. **Powerful.** Provides querying capabilities that combine database queries and application code.
    - Calculate an `age` field from `BirthDate` in your queries.
    - [Set](docs/stems/stems-basics.md#static-setter-methods) a `name` field by splitting into `FirstName` and `LastName` before saving.
    - Raise events on a message bus using [dependency injection](docs/stems/dependency-injection.md).

3. **Configurable.** Customise your conventions, response structure, verb strategies to suit your API needs. Integrate with your web host, ORM and IoC to fit nicely in your solution.

    - Include HTTP `Link` headers in [pagination](docs/endpoints/endpoint-config.md#response) responses.
    - Configure `PUT` or `PATCH` [strategy](docs/endpoints/endpoint-config.md#strategies) for partial updates.
    - Allow `camelCase` or `snake_case` [conventions](docs/endpoints/endpoint-config.md#namingconventionswitcher) in your responses.

You can read more in the [documentation](https://firestorm.readthedocs.org), jump straight into the [tutorials](https://github.com/connellw/Firestorm/wiki/Tutorials) or check out the [samples](https://github.com/connellw/FirestormSamples).

## Install

Firestorm is available from the GitHub repository and as NuGet Packages.

```ps1
PM> Install-Package Firestorm.Endpoints
PM> Install-Package Firestorm.Stems
PM> Install-Package Firestorm.AspNetCore2
PM> Install-Package Firestorm.EntityFrameworkCore2
```

Prerelease packages are available from the [MyGet feed](https://www.myget.org/F/firestorm/api/v3/index.json).

See the [Installation](docs/setup/installation.md) section for more detailed setup information.

## About

Firestorm is a bit of an experiment that grew into something I feel other developers could use. It was never a clearly defined project, but I decided to set some milestones and make it my first open-source project.

It's still in active development. There are a lot of features I want to add!

#### Copyright
Copyright &copy; 2017-2019 Connell Watkins

#### License
Firestorm is licensed under MIT. Refer to [LICENSE.txt](LICENSE.txt) for detailed information.