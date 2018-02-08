using Firestorm.Data;
using Firestorm.Endpoints.Start;
using Firestorm.AspNetCore2;
using Firestorm.Fluent;
using Firestorm.Fluent.Start;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Extensions.AspNetCore
{
    public static class FluentServicesExtensions
    {
        /// <summary>
        /// Configures Firestorm Stems using Stem tpes from the application's entry assembly.
        /// </summary>
        public static IFirestormServicesBuilder AddFluent<TApiContext>(this IFirestormServicesBuilder builder)
            where TApiContext : ApiContext, new()
        {
            var context = new TApiContext();

            builder.AddStartResourceFactory(sp => new FluentStartResourceFactory
            {
                RestContext = context,
                DataSource = sp.GetService<IDataSource>()
            });

            return builder;
        }
    }
}
