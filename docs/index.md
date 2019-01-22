# Welcome to Firestorm

Firestorm is a REST API framework for .NET.

The [aim](intro/aims.md) is to provide neat and easy ways to write standardised APIs.

## Human-readable REST API

Your API will be used by humans too. Designing, debugging and maintaining your API is more difficult if the request or response is too bloated with redundant information.

Firestorm keeps the HTTP request and response concise and understandable.

```http
GET /characters/harry-potter/friends

200 OK
[
    { "name": "Ronald Weasley" },
    { "name": "Hermione Granger" }
]
```

You can read more about the requests and responses [here](endpoints/basic-requests.md).

## Configurable

You can customise many aspects of Firestorm to suit your API needs.

- Response structure. You could add `"status": "ok"` to successful responses.
- Naming conventions. `snake_case`, `camelCase` or `PascalCase`
- Verb strategies. Use `PUT` or `PATCH` for partial updates

These and much more are all enabled by the main [configuration builder](setup/configuration-builder.md).

## Clean and concise code

Writing this kind of REST API using a `Controller` can be tedious. Writing the same CRUD operations in each controller, the same null checks, permissions. That's before you even get into adding `where` filters or pagination.

Firestorm simplifies all that by using definitions of fields used in your API. You tell Firestorm how to write a field, and that logic is used throughout your API.

There are currently two ways to write your APIs: [Stems](stems/stems-intro.md) and [Fluent](fluent/fluent-intro.md).

## Efficient database querying

Firestorm runs on top of your ORM. Using your code and the client's request, it builds an `IQueryable` that is executed by your LINQ Provider.

It takes advantage of deferred execution and asynchronous features within C# and .NET to execute queries efficiently.

See the [solution architecture](contrib/solution-architecture.md) for information on how the project is built.


## Open Source

Firestorm is licensed under MIT. Refer to [LICENSE.txt](../LICENSE.txt) for detailed information.