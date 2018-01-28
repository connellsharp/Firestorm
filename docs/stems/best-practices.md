> But what about my architecture?

There are a few ways to think about how Stems fit into your architecture and different ways to achieve good separation of concerns.

# Reference your Stems in your application

You can create something like a `YourApplication.Stems` project and reference this in your application.

This project only needs to reference `Firestorm.Stems` and needs no knowledge of your application config or Web API framework.

Your application project with the `Startup` class can be configured to find Stems in this library.

You can also include a dependency on `Firestorm.Stems.Roots.Derive`. This library could contain your `RootsResourceFactory`. Alternatively, you can do this in a different project that references the above Stems project. Or you could even put the roots straight into your application project, especially if you're using [DataSource roots]().

# Reference your models in your Stems

> How can I keep my database and business logic separate?

The Firestorm Engine is heavily dependent on LINQ, Expression Trees, `IQueryable` and ultimately, your ORM's query provider. Stems provide the Engine with the Expressions used to build its queries.

Developers debate whether you should abstract out Entity Framework and use your own Unit Of Work and Repository pattern. Firestorm hopes to fit into your way of thinking.

## Depending on your entities

If you fall into either the _"EF is already a Unit Of Work abstraction"_ or, like me,  the _"passing around the `IQueryable<>` is a leaky abstraction anyway"_ camps, then you probably don't mind just hooking your Stems directly to the Entity objects.

This doesn't offend me personally. I try to bend the rules a bit by thinking of the Stem as a business object that wraps the Entity. Basic property mapping is taken care of and more complex logic can be considered business functionality.

If you're okay with it, you could even inject your `IDbSet<>` or `IDbContext<>` into your Stem if you wanted to add more data-driven functionality. Or maybe that's a step too far...

## Depending on your repositories

If you fall into the _"I don't want that reference to EntityFramework in my business logic"_ camp then you'll probably want to map to your domain objects and your repository abstraction.

This makes most sense when using [Derived Roots](Roots/Derived), as each root can pull the `IQueryable` used as the starting point to build a query from your repository abstraction.

## Depending on your services

Well, if your services return `IQueryable` objects, then you can still use the above method.

If your services return arrays, lists or any implementation `IEnumerable`, you could `.AsQueryable()` them, but beware that this could be using LINQ-to-objects to execute your queries in memory. This may be fine for a small static list, perhaps one that contains objects that relate to objects from `IQueryable` stems.

But generally, you might want to consider if you actually need Firestorm here. You might be trying to allow flexible querying for some tightly defined business rules.

