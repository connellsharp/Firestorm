using Firestorm.AspNetCore2;
using Firestorm.Endpoints;
using Firestorm.EntityFrameworkCore2;
using Firestorm.Stems.FunctionalTests.Data;
using Firestorm.Stems.Roots;
using Firestorm.Testing.Data;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Stems.FunctionalTests.Web
{
    public class Startup
    {
        private readonly FunctionalTestConfig _config;

        public Startup(FunctionalTestConfig config)
        {
            _config = config;
        }

        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<MusicDbContext>(builder =>
                {
                    string connectionString = DbConnectionStrings.Resolve("Firestorm.MusicExample");
                    builder.UseSqlServer(connectionString);
                });

            services.AddFirestorm()
                .AddEndpoints(ExampleConfiguration.EndpointConfiguration)
                .AddStems()
                .Add<ITypeGetter>(new NestedTypeGetter(_config.TestClassType))
                .AddEntityFramework<MusicDbContext>();
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            DatabaseInitializer.EnsureInitialized(app);

            app.UseMiddleware<CriticalJsonErrorMiddleware>();
            app.UseMiddleware<SpoofUserMiddleware>();

            app.UseFirestorm();
        }
    }
}