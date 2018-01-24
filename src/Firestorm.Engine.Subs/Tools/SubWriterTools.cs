using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Firestorm.Engine.Subs
{
    /// <summary>
    /// Contains dependencies used by Sub writers and resources.
    /// </summary>
    public class SubWriterTools<TItem, TProperty, TNav>
        where TItem : class
        where TNav : class, new()
    {
        internal Expression<Func<TItem, TProperty>> NavExpression { get; }
        internal IRepositoryEvents<TNav> RepoEvents { get; }
        internal INavigationSetter<TItem, TProperty> Setter { get; }

        public SubWriterTools(Expression<Func<TItem, TProperty>> navExpression, IRepositoryEvents<TNav> repoEvents, [CanBeNull] INavigationSetter<TItem, TProperty> setter)
        {
            NavExpression = navExpression;
            RepoEvents = repoEvents;
            Setter = setter ?? new DefaultNavigationSetter<TItem, TProperty>(NavExpression);
        }
    }
}