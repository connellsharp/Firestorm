# Basic Requests

All query examples will use the default parameter names and delimiters. These can be configured in the [`EndpointConfiguration.QueryString` property](endpoint-config.md#querystring).

## Collections

List items in a collection simply by requesting the collection resource.

```http
GET /people
```
```json
[
    { "id": 1, "name": "Bilbo Baggins", "age": 111 },
    { "id": 2, "name": "Albus Dumbledore", "age": 115 },
    { "id": 3, "name": "Peter Pan", "age": 13 }
]
```

### Filtering

By default, the `where` and `filter` querystring parameters can filter the collection results.

```http
GET /people?where=age=111
```
```json
[
    { "id": 1, "name": "Bilbo Baggins", "age": 111 }
]
```

Other operators can be used in place of the `=`.

- `=`, `==` Equals
- `!=`, `!==`, `<>` Not Equals
- `>` Greater than
- `<` Less than
- `>=` Greater than or equals
- `<=` Less than or equals
- `^=` Starts with
- `$=` Ends with
- `*=` Contains
- `=*` In

For example:

```http
GET /people?where=age>100
```
```json
[
    { "id": 1, "name": "Bilbo Baggins", "age": 111 },
    { "id": 2, "name": "Albus Dumbledore", "age": 115 }
]
```

#### Fields as querysring keys

You can also completely omit the `where` or `filter` parameter and use the field name as the parameter directly.

```http
GET /people?age=111
```

This can even be used with other operations, although the querystring would not appear to be formatted correctly.

```http
GET /people?age>100
```

### Sorting

The `sort`, `order` or `orderby` querystring parameters can all be used to sort the collection results.

The default order is ascending order.

```http
GET /people?sort=age
```
```json
[
    { "id": 3, "name": "Peter Pan", "age": 13 },
    { "id": 1, "name": "Bilbo Baggins", "age": 111 },
    { "id": 2, "name": "Albus Dumbledore", "age": 115 }
]
```

You can specify the sort direction order by adding a delimiter (`+` or ` `) and a modifier (`asc`, `desc`, `ascending`, `descending`).

```http
GET /people?sort=age+desc
```

Multiple sort instructions can be chained using a different delimiter (`,` or `;`). The left-most instruction will be used first, and others will be used if the values are identical.


```http
GET /people?sort=age,id+desc,name
```

### Selecting fields

The `fields` or `select` parameters can specify which fields you want in the response.

Fields are separated by the `,`, `;`, `+` or ` ` delimiters.

```http
GET /people?fields=name,age
```
```json
[
    { "name": "Peter Pan", "age": 13 },
    { "name": "Bilbo Baggins", "age": 111 },
    { "name": "Albus Dumbledore", "age": 115 }
]
```

You can also select hidden fields this way.

```http
GET /people?fields=name,date_of_birth
```

## Items

Identifiers can be used to drill-down into a single item within the collection.

```http
GET /people/3
```
```json
{
    "id": 3,
    "name": "Peter Pan",
    "age": 13
}
```

If an item with the given identifier does not exist, a 404 response will be given.

```http
GET /people/9999
```
```http
404 Not Found
{
    "status": "not_found",
    "error": "identifier_not_found",
    "message": "An item was not found with the identifier '9999'."
}
```

## Selecting fields

The `fields` or `select` parameters can be used just like with collections.

```http
GET /people/2?fields=name,age
```
```json
{
    "name": "Albus Dumbledore",
    "age": 115
}
```

### Editing items

You can perform a _partial_ update of an item using the `PUT` or `PATCH` methods, depending on the server's `EndpointConfiguration.RequestStrategies` property.

```http
PUT /people/2
{
    "name": "Professor Albus Percival Wulfric Brian Dumbledore",
    "age": 116
}
```
```http
200 OK
{
    status: "ok"
}
```

#### Upserting

Even if a resource at this URL with this identifier does not exist, the server may be configured to allow an 'upsert'. This will attempt to create the resource at the URL.

If this is the case, the server will respond with a 201 status code.

```http
201 Created
{
    "status": "created"
}
```

If the URL cannot be edited or created, a 404 will be returned.

## Scalars

Each field can also be navigated to directly. Scalar fields will return just their value on their own.

```http
GET /people/1/name
```
```json
"Bilbo Baggins"
```

Scalars can also be edited using the PUT property.


```http
PUT /people/1/name
"Frodo Baggins"
```
```http
200 OK
{
    "status": "ok"
}
```
