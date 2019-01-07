namespace Firestorm.Fluent
{
    public interface IApiIdentifierBuilder<TItem>
    {

    }
    public interface IApiIdentifierBuilder<TItem, TIdentifier> : IApiIdentifierBuilder<TItem>
    {
        IApiIdentifierBuilder<TItem, TIdentifier> HasName(string identifierName);
    }
}