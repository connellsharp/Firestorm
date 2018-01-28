# What is Firestorm?

Firestorm is a suite of NuGet packages that can be included in your .NET application.

You define the items and fields used in your API, configure the conventions, and as if by magic, a fully-featured REST API appears.


# What can it do?

The aim is to let all the common REST conventions be taken care of by the framework. All you need to do is define your API fields. Whether its `PUT` or `PATCH`, plural or singular, `PascalCase` or `snake_case`, `403` or `404` status code, it doesn't matter.

The framework takes care of querying your collection using the querystring.

- Selecting fields `?fields=id,name`
- Filtering results `?name=Fred` or `?where=created>=2018-01-01`
- Sorting `?sort=name+asc`
- Pagination `?page=2` or `offset=50`

And you've only defined that the `name` field is in fact a combination of `FirstName` and `LastName` in your database **once**. The framework reuses the expression.

It even uses the same expression if the client uses `PUT` to edit the item and the field is read-only. It'll check if the value is the same, and if not, it'll return an error.

Or you could define a setter that splits the given `name` into `FirstName` and `LastName`.

The URL path can be used to identify items.

- Identifying `/authors/123`
- Identifying by another field `/authors/by_name/georgerrmartin`
- Special identifiers `/users/me`

Firestorm also takes care of the response status codes. `404` if the client requests an ID that doesn't exist, `400` if it requests a field that is not defined. You don't have to worry about it!


# Who made Firestorm?

Firestorm is a personal side-project of me, [Connell Watkins](https://twitter.com/connellwatkins). I tinker with little side-projects from time to time, but this one got a little out of hand so I thought I'd release it properly.

For this project I believe open source is the best option. Besides, I've always wanted to delve into open source and see what it's all about. I chose a permissive MIT license for the project to encourage usage and involvement in the project. I hope it helps developers and I hope it can benefit from the community.

Maybe in the future I'll charge for commercial support or build proprietary enterprise addons to monetise this project, but for now that is not my priority. I just want to make Firestorm the best damn REST API framework in existance.