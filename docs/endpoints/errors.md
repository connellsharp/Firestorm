# Errors

If an error occurs while processing a request, the API responds with a standard error body.

The error body is built using rules from your [endpoint response configuration](endpoint-config.md#Response), such as `StatusField` and `ShowDeveloperErrors`.

```json
{
    "error": "authoization",
    "error_description": "You are not authorized to set the 'name' field."
}
```

## Custom Exceptions

If an exception is thrown, Firestorm derives the `error` value from the exception type name and the `error_description` from the exception `Message` property.

```json
{
    "error": "null_reference",
    "error_description": "Object reference not set to an instance of an object."
}
```

If the exception derives from the built-in `RestApiException`, the `StatusCode` property is used to determine the HTTP response status code for this error. For example, you could use `400 Bad Request` for custom validation exceptions.