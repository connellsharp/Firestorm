namespace Firestorm.Features
{
    public interface IFeature<T>
    {
        T Apply(T services);
    }
}