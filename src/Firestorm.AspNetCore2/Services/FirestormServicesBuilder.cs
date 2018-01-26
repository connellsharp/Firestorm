using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.AspNetCore2
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