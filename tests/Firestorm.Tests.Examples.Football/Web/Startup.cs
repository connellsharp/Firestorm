using System;
using Firestorm.Endpoints;
using Firestorm.AspNetCore2;
using Firestorm.AspNetCore2.Middleware;
using Firestorm.Endpoints.Responses;
using Firestorm.Extensions.AspNetCore;
using Firestorm.Tests.Examples.Football.Data;
using Firestorm.Tests.Examples.Football.Models;
using Firestorm.Tests.Examples.Football.Tests;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Tests.Examples.Football.Web
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
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<FootballDbContext>(builder =>
                {
                    const string connection = @"Server=(localdb)\mssqllocaldb;Database=Firestorm.Tests.Examples.Football;Trusted_Connection=True;ConnectRetryCount=0";
                    builder.UseSqlServer(connection);
                });

            var fsBuilder = services.AddFirestorm()
                .AddEntityFramework<FootballDbContext>();

            switch (_tech)
            {
                case FirestormApiTech.Stems:
                    fsBuilder.AddStems();
                    break;

                case FirestormApiTech.Fluent:
                    fsBuilder.AddFluent<FootballRestContext>();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var dbContext = app.ApplicationServices.GetService<FootballDbContext>())
            {
                dbContext.Database.EnsureCreated();
                DbInitializer.Initialize(dbContext);
            }

            app.UseFirestorm(new RestEndpointConfiguration
            {
                ResponseConfiguration =
                {
                    StatusField = ResponseStatusField.StatusCode,
                    PageConfiguration =
                    {
                        UseLinkHeaders = true
                    }
                }
            });
        }
    }
}