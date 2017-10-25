using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Firestorm.Tests.Integration.AspNetCore
{
    public class NetCoreIntegrationStartup
    {
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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