using System;
using System.Linq.Expressions;

namespace Firestorm.Fluent
{
    public class ApiFieldBuilder<TItem, TField> : IApiFieldBuilder<TItem>
    {
        public Expression<Func<TItem, TField>> Expression { get; }

        internal string Name { get; private set; }

        public ApiFieldBuilder(Expression<Func<TItem, TField>> expression)
        {
            Expression = expression;
        }

        public ApiFieldBuilder<TItem, TField> HasName(string fieldName)
        {
            Name = fieldName;
            return this;
        }
    }
}