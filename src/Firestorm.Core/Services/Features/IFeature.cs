namespace Firestorm.Features
{
    public interface IFeature<T>
    {
        T AddTo(T services);
    }
}