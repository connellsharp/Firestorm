using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Endpoints.AspNetCore
{
    internal class FirestormServicesBuilder : IFirestormServicesBuilder
    {
        public IServiceCollection Services { get; }

        public FirestormServicesBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}