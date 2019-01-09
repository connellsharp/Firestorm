using Firestorm.AspNetCore2;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Configuration;
using Firestorm.EntityFrameworkCore2;
using Firestorm.FunctionalTests.Data;
using Firestorm.FunctionalTests.Setup;
using Firestorm.Testing.Data;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.FunctionalTests.Web
{
    public class Startup
    {
        private readonly IStartupConfigurer _tech;

        public Startup(IStartupConfigurer tech)
        {
            _tech = tech;
        }

        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<FootballDbContext>(builder =>
                {
                    string connectionString = DbConnectionStrings.Resolve("Firestorm.FootballExample");
                    builder.UseSqlServer(connectionString);
                });

            var fsBuilder = services.AddFirestorm()
                .AddEndpoints(new DefaultRestEndpointConfiguration
                {
                    ResponseConfiguration =
                    {
                        StatusField = ResponseStatusField.StatusCode,
                        PageConfiguration =
                        {
                            UseLinkHeaders = true
                        },
                        ShowDeveloperErrors = true
                    }
                })
                .AddEntityFramework<FootballDbContext>();

            _tech.Configure(fsBuilder);
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            DatabaseInitializer.EnsureInitialized(app);

            app.UseMiddleware<CriticalJsonErrorMiddleware>();

            app.UseFirestorm();
        }
    }
}