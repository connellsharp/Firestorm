# Pagination

The default will use `sort` and `where` to automatically browse to the next page. If you're sorting by `name+asc`, the response contains the first 100 results and the last item is the name `Frank`, the next page will be a filter `where=name>Frank`.

The Pagination category will be configurable. We'd want to support:

- The default `where` and `sort` technique.
- A classic `page=2` parameter. Maybe with `size=100` parameter.
- Another classic `limit` and `offset` parameters. Or `skip` and `take`.

## Link header

The Endpoints library will use the page info to return a `Link` header that can be followed to get the next page.

```http
Link: <https://api.yourapplication.com/people?sort=name+asc&where=name%3EFrank>; rel="next"
```

## Wrapped response

This will most likely need to tie into the `ResponseGenerator` to get page info in the response body. Something like:

```http
GET /people?sort=name
```
```json
{
    items: [
         { id: 1, name: "Aaron" },
        ...
         { id: 1234, name: "Frank" },
    ],
    page: {
        next: "/people?sort=name&where=name>Frank"
    }
}
```

## Prev problem

Automatically finding a query to get the page before could prove to be difficult.

One option is to use a new querystring param to specify that the results are paged to the end of the sort. For example, on the `>Frank` page:

```http
GET /people?sort=name&where=name>Frank
```
```json
{
    items: [
         { id: 1235, name: "Fred" },
        ...
         { id: 2345, name: "Jo" },
    ],
    page: {
        prev: "/people?where=name<Frank&sort=name+asc&page=end",
        next: "/people?where=name>Jo&sort=name+asc"
    }
}
```

Another option is to include it as a third sort modifier. Thought of like *"sort descending, then limit results, then sort ascending"*. This also forces a sort order to be specified this way, which resolves the problem where with SQL servers that don't use `LIMIT` and `OFFSET`.

```json
        prev: "/people?where=name<Frank&sort=name+asc+end"
```

Or perhaps to add further control, a parameter to specify sort instructions after the limiting:

```json
        prev: "/people?where=name<Frank&sort=name+desc&postsort=name+asc"
```