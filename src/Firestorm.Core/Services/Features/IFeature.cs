namespace Firestorm.Features
{
    public interface IFeature<T>
    {
        void AddTo(T services);
    }
}