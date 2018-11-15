using Firestorm.AspNetCore2;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Web;
using Firestorm.Host;
using Firestorm.Tests.Integration.Http.Base;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Firestorm.Tests.Integration.Http.AspNetCore
{
    /// <summary>
    /// A startup class that uses the AddFirestorm method with the configureAction to configure the options.
    /// </summary>
    public class NetCoreServicesStartup
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

    /// <summary>
    /// A startup class that configures the <see cref="IStartResourceFactory"/> independently, then simply calls UseFirestorm with no params.
    /// </summary>
    public class NetCoreServicesWithOptionsStartup
    {
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DefaultRestEndpointConfiguration>(config =>
            {
                config.ResponseConfiguration.ShowDeveloperErrors = true;
                //
            });
            
            services.AddFirestorm()
                .AddEndpoints(sp => sp.GetService<IOptions<DefaultRestEndpointConfiguration>>().Value)
                .AddStartResourceFactory(new IntegratedStartResourceFactory());
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseFirestorm();
        }
    }
}