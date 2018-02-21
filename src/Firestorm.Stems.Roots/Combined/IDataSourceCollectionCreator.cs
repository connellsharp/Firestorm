namespace Firestorm.Stems.Roots.Combined
{
    internal interface IDataSourceCollectionCreator
    {
        IRestCollection GetRestCollection(Stem stem);
    }
}