using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Firestorm.Fluent
{
    public abstract class ApiContext
    {
        protected internal virtual void OnModelCreating(ApiBuilder apiBuilder)
        {
        }
    }
}
