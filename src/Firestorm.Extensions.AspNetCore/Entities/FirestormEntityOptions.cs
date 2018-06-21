namespace Firestorm.Extensions.AspNetCore
{
    public class FirestormEntityOptions
    {
        /// <summary>
        /// If this option is enabled, every Firestorm API request will call <see cref="Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade.EnsureCreated"/>.
        /// This can be handy in development with local databases.
        /// </summary>
        public bool EnsureCreatedOnRequest { get; set; }
    }
}