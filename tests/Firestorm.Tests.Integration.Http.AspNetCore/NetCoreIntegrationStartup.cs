using Firestorm.AspNetCore2;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Start;
using Firestorm.Endpoints.Web;
using Firestorm.Tests.Integration.Http.Base;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Tests.Integration.Http.AspNetCore
{
    /// <summary>
    /// A startup class with no services, all defined in the main config object.
    /// </summary>
    public class NetCoreOriginalConfigStartup
    {
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseFirestorm(new FirestormConfiguration
            {
                StartResourceFactory = new IntegratedStartResourceFactory(),
                EndpointConfiguration = new DefaultRestEndpointConfiguration
                {
                    ResponseConfiguration = new ResponseConfiguration
                    {
                        ShowDeveloperErrors = true
                    }
                }
            });
        }
    }

    /// <summary>
    /// A startup class that uses the AddFirestorm method with the configureAction to configure the options.
    /// </summary>
    public class NetCoreServicesOptionsStartup
    {
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFirestorm(config =>
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
    public class NetCoreServicesSingletonsStartup
    {
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IStartResourceFactory>(new IntegratedStartResourceFactory());
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseFirestorm(config =>
            {
                config.EndpointConfiguration.ResponseConfiguration.ShowDeveloperErrors = true;
                //
            });
        }
    }
}