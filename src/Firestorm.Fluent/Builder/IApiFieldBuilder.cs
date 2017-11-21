namespace Firestorm.Fluent
{
    public interface IApiFieldBuilder<TItem>
    { }

    public interface IApiFieldBuilder<TItem, TField> : IApiFieldBuilder<TItem>
    {
        IApiFieldBuilder<TItem, TField> HasName(string fieldName);
    }
}