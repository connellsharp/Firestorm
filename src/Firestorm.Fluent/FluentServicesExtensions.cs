﻿using System;
using Firestorm.Data;
using Firestorm.Host;

namespace Firestorm.Fluent
{
    public static class FluentServicesExtensions
    {
        /// <summary>
        /// Configures Firestorm Fluent API using the given context type.
        /// </summary>
        public static IServicesBuilder AddFluent<TApiContext>(this IServicesBuilder builder)
            where TApiContext : ApiContext, new()
        {
            var context = new TApiContext();
            return builder.AddFluent(context);
        }
        
        /// <summary>
        /// Configures Firestorm Fluent API using a <see cref="buildAction"/> delegate.
        /// </summary>
        public static IServicesBuilder AddFluent(this IServicesBuilder builder, Action<IApiBuilder> buildAction)
        {
            var context = new DelegateApiContext(buildAction);
            return builder.AddFluent(context);
        }
        
        /// <summary>
        /// Configures Firestorm Fluent API using the registered service for <see cref="IApiContext"/>.
        /// </summary>
        public static IServicesBuilder AddFluent(this IServicesBuilder builder)
        {
            builder.AddStartResourceFactory(sp => new FluentStartResourceFactory
            {
                ApiContext = sp.GetService<IApiContext>(),
                DataSource = sp.GetService<IDataSource>()
            });

            return builder;
        }
        
        /// <summary>
        /// Configures Firestorm Fluent API by automatically finding the root item types and automatically configuring them.
        /// </summary>
        public static IServicesBuilder AddFluent(this IServicesBuilder builder, AutoConfiguration configuration)
        {
            builder.AddDataSourceTypeFinder();
            
            builder.AddStartResourceFactory(sp => new FluentStartResourceFactory
            {
                ApiContext = new AutomaticApiContext(sp.GetService<IItemTypeFinder>().FindItemTypes(), configuration),
                DataSource = sp.GetService<IDataSource>()
            });

            return builder;
        }
        
        /// <summary>
        /// Configures Firestorm Fluent API using a <see cref="apiContext"/>.
        /// </summary>
        public static IServicesBuilder AddFluent(this IServicesBuilder builder, IApiContext apiContext)
        {
            builder.AddStartResourceFactory(sp => new FluentStartResourceFactory
            {
                ApiContext = apiContext,
                DataSource = sp.GetService<IDataSource>()
            });

            return builder;
        }
    }
}
