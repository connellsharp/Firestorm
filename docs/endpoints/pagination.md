# Pagination

The Pagination strategy is configurable.

- Use `sort` and `where` automatically e.g. `sort=name&where=name>Frank`.
- A classic `page=2` parameter. Maybe with `size=100` parameter.
- Another classic `limit` and `offset` parameters. Or `skip` and `take`.

## Link header

Firestorm can be configured to return a `Link` header that can be followed to get the next page.

```http
HTTP/1.1 200 OK
Link: <https://api.yourapplication.com/people?sort=name+asc&where=name%3EFrank>; rel="next"

[
	 { "id": 1, "name": "Aaron" },
	...
	 { "id": 1234, "name": "Frank" }
]
```

## Wrapped response

You can also configure the response body to include the page info.

```http
GET /people?sort=name HTTP/1.1

HTTP/1.1 200 OK
{
    items: [
         { "id": 1, "name": "Aaron" },
        ...
         { "id": 1234, "name": "Frank" }
    ],
    page: {
        "next": "/people?sort=name&where=name>Frank"
    }
}
```