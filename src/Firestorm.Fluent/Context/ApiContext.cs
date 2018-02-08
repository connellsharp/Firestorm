﻿namespace Firestorm.Fluent
{
    public abstract class ApiContext
    {
        protected ApiContext(ApiContextOptions options)
        {
            Options = options;
        }

        protected ApiContext()
            : this(new ApiContextOptions())
        { }

        protected internal virtual void OnApiCreating(IApiBuilder apiBuilder)
        {
        }

        protected internal ApiContextOptions Options { get; }
    }
}