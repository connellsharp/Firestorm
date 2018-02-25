using System;
using Firestorm.Data;
using Firestorm.AspNetCore2;
using Firestorm.Fluent;
using Firestorm.Fluent.Start;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Extensions.AspNetCore
{
    public static class FluentServicesExtensions
    {
        /// <summary>
        /// Configures Firestorm Fluent API using the given context type.
        /// </summary>
        public static IFirestormServicesBuilder AddFluent<TApiContext>(this IFirestormServicesBuilder builder)
            where TApiContext : ApiContext, new()
        {
            var context = new TApiContext();

            builder.AddStartResourceFactory(sp => new FluentStartResourceFactory
            {
                ApiContext = context,
                DataSource = sp.GetService<IDataSource>()
            });

            return builder;
        }
        
        /// <summary>
        /// Configures Firestorm Fluent API using a <see cref="buildAction"/> delegate.
        /// </summary>
        public static IFirestormServicesBuilder AddFluent(this IFirestormServicesBuilder builder, Action<IApiBuilder> buildAction)
        {
            var context = new DelegateApiContext(buildAction);
            
            builder.AddStartResourceFactory(sp => new FluentStartResourceFactory
            {
                ApiContext = context,
                DataSource = sp.GetService<IDataSource>()
            });

            return builder;
        }
    }
}
