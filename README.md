> *Note:* Firestorm is in alpha development phase.

# Firestorm

Firestorm is a REST API framework for .NET. The aim is to provide a neat and easy ways to write standardised APIs with more of the leg-work taken care of.

_Using **[Stems]()** to describe your API_

```csharp
public class ArtistsStem : Stem<Artist>
{
    [Get, Identifier]
    public static int ID { get; }

    [Get, Set]
    public static string Name { get; }
}
```

_Exposes RESTful endpoints_

```
GET /artists/123
```
```json
{
    "id": 321,
    "name": "Noisia"
}
```

## Features

1. **Clean.** Lets you write neat and concise code to describe your API and exposes lightweight, human-readable responses.

2. **Powerful.** Provides querying capabilities that combines database queries and application code.

3. **Configurable.** Customise your conventions, response structure, verb strategies to suit your API needs. Integrate with your web host, ORM and IoC to fit nicely in your solution.

4. **Agile.** It's easy to start a basic project with just a few endpoints and grow rapidly as requirements build.

## Install

Firestorm is available from the GitHub repository and as NuGet Packages.

```
PM> Install-Package Firestorm.Stems
PM> Install-Package Firestorm.Endpoints.AspNetCore
PM> Install-Package Firestorm.Engine.EntityFramework
```

See the [Getting Started]() section for more detailed setup information.

## About

Firestorm is a bit of an experiment that grew into something I feel other developers could use. It was never a clearly defined project, but I decided to set some milestones and make it my first open-source project.

It's still in active development. I have so many feature ideas for future versions! Over the months, I want to take this project to great places, advancing my skills and (hopefully) benefitting the community whilst doing so!

#### Copyright
Copyright © 2017 Connell Watkins

#### License
Firestorm is licensed under MIT. Refer to [LICENSE.txt](https://github.com/connellw/Firestorm/blob/master/LICENSE.txt) for detailed information.
