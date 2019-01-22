# Endpoint Configuration

All Firestorm applications will need to call `AddEndpoints` to configure the interaction between the API request and Firestorm resources.

The default configuration is used if no arguments are given. Other overloads are available to configure the `EndpointConfiguration` object.

```csharp
services.AddFirestorm()
    .AddEndpoints(c =>
    {
        // configure endpoints here
    });
```

## Response

The `Response` property defines how responses are built before they are sent to the client. This object contain further properties.

```c#
.AddEndpoints(c =>
{
    c.Response.ShowDeveloperErrors = true;
    c.Response.StatusField = ResponseStatusField.StatusCode;
});
```

- `ShowDeveloperErrors` includes stack trace and inner exceptions in error responses.
- `StatusField` sets a field used in the response to a command.
  - A value of `ResponseStatusField.StatusCode` includes a `status` property, which can be `ok`, `created` or `error`.
  - A value of `ResponseStatusField.SuccessBoolean` includes a `success` boolean property.
- `WrapResourceObject` also includes the above status field in query responses.
- `Pagination` provides further options for paging collections.
  - `MaxPageSize` sets the count of items per page.
  - `UseLinkHeaders` responds with a `Link` HTTP header for the client to follow to the next and previous pages.
  - `WrapCollectionResponseBody` responds with a paging object that contains properties for the page info with the full collection nested inside.
  - `SuggestedNavigationType` defines how the next and previous page URLs are generated.
    - A value of `PageNavigationType.PageNumber` simply uses a querystring of `?page=2`.
    - A value of `PageNavigationType.Offset` uses `?offset=100`.
    - A value of `PageNavigationType.SortAndFilter` attempts to find the page by filters e.g. `?sort=id+asc&where=id>123`.
- `ResourceOnSuccessfulCommand` forces commands to respond with the modified resource, instead of a simple acknowledement body.

## QueryString

The `QueryString` is another object that defines which keywords and operators can be used in querystrings.

- `SelectFieldQueryKeys` for projecting fields. Defaults to `[ "select", "fields" ]`.
- `SelectFieldDelimiters` defines characters used to split the fields in the above value. Defaults to `[ ',', ';', '+', ' ' ]`.
- `WhereFilterQueryKeys` for filters. Defaults to `[ "where", "filter" ]`.
- `SpecialFilterKeysEnabled` allows filters to work directly in the querystring like `?id=123`.
- `WhereFilterComparisonOperators` is a dictionary of strings (e.g. `"!="`) to `FilterComparisonOperator` values.
- `SortOrderQueryKeys` for sorting. Defaults to `[ "sort", "order", "orderby" ]`.
- `SortDirectionModifiers` is a dictionary of strings (e.g. `asc`) to `SortDirection` values.
- `SortInstructionDelimiters` defines characters used to split the sort fields. Defaults to `[ ',', ';' ]`.
- `SortModifierDelimiters` defines characters used to split each field with a modifer. Defaults to `[ '+', ' ' ]`.
- `PageSizeQueryKeys` for filters. Defaults to `[ "limit", "take", "size", "per_page" ]`.
- `PageOffsetQueryKeys` for filters. Defaults to `[ "offset", "skip" ]`.
- `PageNumberQueryKeys` for filters. Defaults to `[ "page" ]`.
- `SpecialPageNumbers` is a dictionary of strings that are translated to page numbers e.g. `first` maps to page `1`.
- `DictionaryReferencePrefix` can be used in the URL path to turn a collection into a dictionary e.g. `/people/by_name/`.

## Strategies

The `Strategies` property contains several strategy sets defining what *unsafe* methods (e.g. POST, PUT, PATCH) should do to the different resource types.

For example, you can configure the `DELETE` method on a collection to clear the whole collection.

```c#
.AddEndpoints(c =>
{
    c.Strategies.ForCollections[UnsafeMethod.Delete] = new ClearCollectionStrategy();
});
```

## NamingConventionSwitcher

The `NamingConventionSwitcher` converts the client request field names to your .NET member names.

The default `NamingConventionSwitcher` class has 3 properties specifying the cases to convert from and to.

- `CodedCase` is the case used for your .NET members. This defaults to `PascalCaseConvention`.
- `DefaultOutputCase` is how the clients see your API fields. This defaults to `SnakeCaseConvention`.
- `AllowedCases` is an `IEnumerable<ICaseConvention>` that is used to check input fields from API clients.

There's also the `VoidNamingConventionSwitcher`, which does no switching and makes your API all the same cases as your .NET members.