using Firestorm.Endpoints;
using Firestorm.Host;
using Firestorm.Testing.Http;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.AspNetCore2.IntegrationTests
{
    /// <summary>
    /// A startup class that uses the AddFirestorm method with the configureAction to configure the options.
    /// </summary>
    public class StandardStartup
    {
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFirestorm()
                .AddEndpoints(config =>
                {
                    config.ResponseConfiguration.ShowDeveloperErrors = true;
                    //
                })
                .AddStartResourceFactory(new IntegratedStartResourceFactory());
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseFirestorm();
        }
    }
}