using Firestorm.Engine.Deferring;
using JetBrains.Annotations;

namespace Firestorm.Engine
{
    /// <summary>
    /// Represents a non-engine item that has been returned as if it were a scalar value.
    /// </summary>
    /// <remarks>
    /// TODO shouldn't actually be scalar, but it is basically the same thing atm.
    /// We want to add stuff in for naming convention switching and individual field selecting in querystring.
    /// </remarks>
    public class EngineRestPoco<TItem> : EngineRestScalar<TItem>
        where TItem : class
    {
        public EngineRestPoco([NotNull] IEngineContext<TItem> context, [NotNull] IDeferredItem<TItem> item, [NotNull] INamedField<TItem> field) : base(context, item, field)
        { }
    }
}