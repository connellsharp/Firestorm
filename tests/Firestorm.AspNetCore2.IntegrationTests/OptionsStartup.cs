using Firestorm.Endpoints;
using Firestorm.Host;
using Firestorm.Testing.Http;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServiceProviderServiceExtensions = Firestorm.Host.ServiceProviderServiceExtensions;

namespace Firestorm.AspNetCore2.IntegrationTests
{
    /// <summary>
    /// A startup class that configures the <see cref="IStartResourceFactory"/> independently, then simply calls UseFirestorm with no params.
    /// </summary>
    public class OptionsStartup
    {
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DefaultEndpointConfiguration>(config =>
            {
                config.Response.ShowDeveloperErrors = true;
                //
            });
            
            services.AddFirestorm()
                .AddEndpoints(sp => ServiceProviderServiceExtensions.GetService<IOptions<DefaultEndpointConfiguration>>(sp).Value)
                .AddStartResourceFactory(new IntegratedStartResourceFactory());
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseFirestorm();
        }
    }
}