using Firestorm.AspNetCore2;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Configuration;
using Firestorm.Host;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Client.IntegrationTests
{
    /// <summary>
    /// A startup class that uses the AddFirestorm method with the configureAction to configure the options.
    /// </summary>
    public class Startup
    {  
        public const int Port = 2225;

        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFirestorm()
                .AddEndpoints(config =>
                {
                    config.Response.ShowDeveloperErrors = true;
                    config.Response.StatusField = ResponseStatusField.SuccessBoolean;
                })
                .AddStartResourceFactory(new DoubleTestStartResourceFactory("http://localhost:" + Port));
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseFirestorm();
        }
    }
}