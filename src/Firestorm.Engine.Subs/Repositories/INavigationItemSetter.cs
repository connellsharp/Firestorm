namespace Firestorm.Engine.Subs.Repositories
{
    public interface INavigationItemSetter<in TParent, in TNav>
    {
        void SetNavItem(TParent parent, TNav item);
    }
}