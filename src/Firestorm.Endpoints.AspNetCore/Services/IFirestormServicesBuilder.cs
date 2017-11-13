using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Endpoints.AspNetCore
{
    /// <summary>
    /// An interface that can be extended to inject Firestorm services.
    /// </summary>
    public interface IFirestormServicesBuilder
    {
        IServiceCollection Services { get; }
    }
}