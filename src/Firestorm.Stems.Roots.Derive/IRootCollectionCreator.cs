namespace Firestorm.Stems.Roots.Derive
{
    internal interface IRootCollectionCreator
    {
        IRestCollection GetRestCollection(Root root, Stem stem);
    }
}