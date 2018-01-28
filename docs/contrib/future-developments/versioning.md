# Versioning

## HTTP Request

We want to allow configure one or many versioning strategies on your API.

We'll allow all the _wrong ways_ described in this article: https://www.troyhunt.com/your-api-versioning-is-wrong-which-is/. And some other, even worse ways.

##### 1. URL

```http
GET /v1.0/people/123
```

##### 2. Custom request header

Configurable custom request header.

```http
Version: 1.0
GET /people/123
```

##### 3. Accept header

```http
Accept: application/vnd.yourapplication.v2+json
GET /people/123
```

##### 4. Querystring

Configuration querystring parameter.

```http
GET /people/123?version=1.0
```

## Config

All should be configurable in the endpoint config. Config object something like this:

```csharp
VersioningStrategy = new VersioningStrategy
{
    AllowUrlVersion = true,
    CustomRequestHeaders = new[] { "Version", "X-Version" },
    AcceptVendorHeaderTypes = new[] { "yourapplication" }
    QuerystringParameters = new [] { "version", "_version", "api-version" },
    NoVersionBehaviour = NoVersionBehaviour.LatestVersion // VersionOne, ThrowNotFound
}
```

## C# Stems

In Stems, we want to be able to deprecate fields and add new fields in specific versions.

A new `Version` attribute can use parameters for the `ComparisonOperator` declared in `Firestorm.Core` and a string version number.

```csharp
public class ArtistsStem : Stem<Artist>
{
    [Get]
    [Identifier]
    public static int ID { get; }

    [Get("Name")]
    [Version(ComparisonOperator.LessThan, "1.1")]
    public static string OldName { get; }

    [Get("Name")]
    [Version(ComparisonOperator.GreaterThanOrEqual, "1.1")]
    public static string NewName { get; }
}
```