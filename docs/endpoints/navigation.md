# Navigation

Each endpoint type has a unique way of navigating into sub-directories to expose further endpoints.

## Directories

The root of your API is a **Directory**. This simply outlines the possible **Collections** you can use, and allows you to navigate into them.

```http
GET /

200 OK
[
    { "name": "artists", "type" : "collection" },
    { "name": "albums", "type" : "collection" },
    { "name": "tracks", "type" : "collection" }
]
```

The `name` field shows the string you can use in the URL to navigate into that collection from this directory, e.g. `/artists`, `/albums` or `/tracks`.

## Collections

Collections list the available **Items** you can navigate into.

```http
GET /artists

200 OK
[
    { "id": 122 },
    { "id": 123 },
    { "id": 124 }
]
```

Collections use an **Identifier** field, defined in your ([Stems](../stems/stems-intro.md) or [Fluent](../fluent/fluent-intro.md)) code. This is conventionally an `id` field, or perhaps a unique `name` field. Specifying the value of an identifier in the URL navigates into an **Item**.

Additionally, you can use the `by_` prefix to navigate into a **Dictionary**. This groups the collection by a field and displays the results as an object with a field for each value. Dictionaries can be further navigated by the value, which means these can be used to specify which identifier to use to find an item too, e.g. `/artists/by_name/noisia`.

## Items

Items are single entities within a collection.

```http
GET /artists/123

200 OK
{
    "id": 123,
    "name": "Noisia"
}
```

Field names can be drilled-into to return the single value of that field. These values could be sub-collections, sub-items or simply **Scalar** values.

Sub-collections and sub-items provide further navigation. These can all be chained together into longer URLs, e.g. `/artists/123/tracks/456/artists`.

## Scalars

Currently, scalar values can't be navigated into any further.

```http
GET /artists/123/name

200 OK
Noisia
```