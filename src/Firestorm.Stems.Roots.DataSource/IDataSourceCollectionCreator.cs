namespace Firestorm.Stems.Roots.DataSource
{
    internal interface IDataSourceCollectionCreator
    {
        IRestCollection GetRestCollection(Stem stem);
    }
}