namespace Firestorm.Features
{
    /// <summary>
    /// Customizes a service registered with <see cref="IServicesBuilder"/>
    /// </summary>
    public interface ICustomization<T>
    {
        T Apply(T services);
    }
}