## Aims

The project is still under development and many of its core aims have yet to materialise.

Firestorm aims to:

**1. Create standardised REST APIs with little setup time.**

You shouldn't have to worry about when to use nouns or verbs or when to pluralise your names. Nor should you worry about which status codes to use, how to setup relationships between your resources, filtering, sorting, pagination. Firestorm aims to take care of all that for you.

**2. Provide a neat and easy way to describe many aspects of your API in your C# code.**

Your API might end up very feature-rich. You may want to authorise individual fields to different scopes. Or maybe deprecate one field in favour of another. Provide a self-documenting API. But surely all that would get quite messy? [Firestorm Stems](../stems/stems-intro.md) aim to keep all that functionality in neat and tidy decorators.

**3. Allow building of powerful queries and application logic out-of-the-box.**

Your querystrings allow select, filters, sort, pagination. But if you want to include some heavy application-logic, wouldn't you have to write some very expensive Linq-to-objects code? The [Firestorm Engine](Engine) combines the best of both worlds to execute complex queries efficiently.

**4. Give the API consumer more control. Let the client choose formatting and conventions.**

A basic API sets the standards for the clients. Your clients want JSON but your endpoint is XML. Your clients use `PascalCase` but your fields are `snake_case`. [Firestorm Endpoints](../endpoints/basic-requests.md) aim to allow the client to use their own conventions.

**5. Expose clean, human-readable, semantic requests and responses. Never get too bloated.**

Frameworks like _OData_ and _ORDS_ are very powerful tools, but they have a far more tolerant definition of "human-readable". [Firestorm Endpoints](../endpoints/basic-requets.md) aim to keep HTTP requests and responses looking like a human developed the API for other humans to read.

**6. Allow for easy extension and modification of your API.**

Your requirements are going to change over time. You'll need to add new fields and deprecate others without breaking the contract with your existing clients. Or you may want to move some endpoints to other microservices. [Firestorm Stems](../stems/stems-intro.md) aim to make all this a breeze so you can develop the awesome API you want, not just one you're stuck with.

**7. Adhere to best practices.**

You love clean software design, don't you? You want to stick to SOLID principals? Firestorm aims to support you in this, even though it has to bend the rules from time to time. [Stems are testable](/Tutorials/Stems/Unit-tests) through dependency injection.

**8. Work with your existing architecture and frameworks.**

Maybe you're already heavily invested in a specific DI framework like Ninject. Maybe you already have mapping rules set out with AutoMapper. Or maybe you're just stuck in your ways. Even if frameworks are not officially supported, Firestorm's [component-based architecture](../contrib/solution-architecture.md) aims to allow the community to write extensions to support other frameworks.