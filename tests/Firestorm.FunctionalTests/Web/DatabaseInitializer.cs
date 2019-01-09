using Firestorm.FunctionalTests.Data;
using Firestorm.Host;
using Microsoft.AspNetCore.Builder;

namespace Firestorm.FunctionalTests.Web
{
    internal static class DatabaseInitializer
    {
        private static readonly object _lock = new object();
        private static bool _isInitialized = false;

        internal static void EnsureInitialized(IApplicationBuilder app)
        {
            lock (_lock)
            {
                if (_isInitialized)
                    return;

                using (var dbContext = app.ApplicationServices.GetService<FootballDbContext>())
                {
                    dbContext.Database.EnsureCreated();
                    DbInitializer.Initialize(dbContext);
                }

                _isInitialized = true;
            }
        }
    }
}