using Firestorm.Endpoints.AspNetCore;
using Firestorm.Endpoints.AspNetCore.Middleware;
using Firestorm.Extensions.AspNetCore;
using Firestorm.Tests.Examples.Football.Data;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Tests.Examples.Football.Web
{
    public class Startup
    {
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFirestorm()
                .AddEntityFramework<FootballDbContext>()
                //.AddFluent<FootballApiContext>()
                .AddStems();
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseFirestorm();
        }
    }
}