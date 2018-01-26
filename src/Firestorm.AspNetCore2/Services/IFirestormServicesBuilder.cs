using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.AspNetCore2
{
    /// <summary>
    /// An interface that can be extended to inject Firestorm services.
    /// </summary>
    public interface IFirestormServicesBuilder
    {
        IServiceCollection Services { get; }
    }
}