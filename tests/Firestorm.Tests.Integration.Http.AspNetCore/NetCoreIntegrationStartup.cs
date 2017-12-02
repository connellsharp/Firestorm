using Firestorm.Endpoints;
using Firestorm.Endpoints.AspNetCore;
using Firestorm.Endpoints.AspNetCore.Middleware;
using Firestorm.Endpoints.Start;
using Firestorm.Tests.Integration.Http.Base;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Firestorm.Tests.Integration.Http.AspNetCore
{
    public class NetCoreIntegrationStartup
    {
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseFirestorm(new FirestormConfiguration
            {
                StartResourceFactory = new IntegratedStartResourceFactory()
            });

            if (env.IsDevelopment())
            {
                // In Development, use the developer exception page
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // In Staging/Production, route exceptions to /error
                //app.UseExceptionHandler("/error");
            }

            var contentRootPath = env.ContentRootPath;
        }
    }
}