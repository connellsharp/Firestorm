using Firestorm.Host;
using Firestorm.Stems.FunctionalTests.Data;
using Microsoft.AspNetCore.Builder;

namespace Firestorm.Stems.FunctionalTests.Web
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

                using (var dbContext = app.ApplicationServices.GetService<MusicDbContext>())
                {
                    dbContext.Database.EnsureDeleted();
                    dbContext.Database.EnsureCreated();
                    
                    MusicDbInitializer.Initialize(dbContext);
                }

                _isInitialized = true;
            }
        }
    }
}