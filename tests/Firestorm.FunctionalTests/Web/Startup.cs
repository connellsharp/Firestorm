using System;
using Firestorm.AspNetCore2;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Web;
using Firestorm.Extensions.AspNetCore;
using Firestorm.Fluent;
using Firestorm.FunctionalTests.Data;
using Firestorm.FunctionalTests.Tests.Setup;
using Firestorm.Stems;
using Firestorm.Testing.Data;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceProviderServiceExtensions = Firestorm.Host.ServiceProviderServiceExtensions;

namespace Firestorm.FunctionalTests.Web
{
    public class Startup
    {
        private readonly FirestormApiTech _tech;

        public Startup(StartupTechSettings tech)
        {
            _tech = tech.Tech;
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

            switch (_tech)
            {
                case FirestormApiTech.Stems:
                    fsBuilder.AddStems();
                    break;

                case FirestormApiTech.Fluent:
                    fsBuilder.AddFluent<FootballApiContext>();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var dbContext = ServiceProviderServiceExtensions.GetService<FootballDbContext>(app.ApplicationServices))
            {
                dbContext.Database.EnsureCreated();
                DbInitializer.Initialize(dbContext);
            }

            app.UseMiddleware<CriticalJsonErrorMiddleware>();

            app.UseFirestorm();
        }
    }
}