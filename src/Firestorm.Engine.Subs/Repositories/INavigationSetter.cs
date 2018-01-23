namespace Firestorm.Engine.Subs.Repositories
{
    /// <summary>
    /// Sets the object value of a navigation property of a parent item.
    /// </summary>
    public interface INavigationSetter<in TParent, in TNav>
    {
        void SetNavItem(TParent parent, TNav item);
    }
}