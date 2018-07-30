# Write Operations

All examples we will describe the default parameter names and delimiters. These can be configured in the `EndpointConfiguration.QueryStringConfiguration` property.

## Adding items to collections

You can add a new item using the `POST` method.

```http
POST /people
{
    "name": "Eddard Stark",
    "age": 36
}
```

Depending on how the server is configured in the `EndpointConfiguration.ResponseContentGenerator` property, the server might return a JSON body response containing the new ID.

The URL to the newly created resource will be in the Location header.

```http
201 Created
Location: /people/4
{
    status: "created",
    id: 4
}
```

## Editing items

You can perform a _partial_ update of an item using the `PUT` or `PATCH` methods, depending on the server's `EndpointConfiguration.RequestStrategies` property.

```http
PUT /people/2
{
    name: "Professor Albus Percival Wulfric Brian Dumbledore",
    age: 116
}
```
```http
200 OK
{
    status: "ok"
}
```

Even if a resource at this URL with this identifier does not exist, the server may be configured to allow an 'upsert'. This will attempt to create the resource at the URL.

If this is the case, the server will respond with a 201 status code.

```http
201 Created
{
    "status": "created"
}
```

If the URL cannot be edited or created, a 404 will be returned.


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

