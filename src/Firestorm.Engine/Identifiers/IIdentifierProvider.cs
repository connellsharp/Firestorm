namespace Firestorm.Engine.Identifiers
{
    /// <summary>
    /// Provides data about URL paths used to identify items within a collection.
    /// </summary>
    public interface IIdentifierProvider<TItem>
    {
        IIdentifierInfo<TItem> GetInfo(string identifierName);
    }
}