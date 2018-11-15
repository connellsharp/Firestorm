using System;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.AspNetCore2
 {
     public static class FirestormServicesExtensions
     {
         /// <summary>
         /// Adds Firestorm services.
         /// </summary>
         public static IFirestormServicesBuilder AddFirestorm(this IServiceCollection services)
         {
             IFirestormServicesBuilder builder = new AspNetCoreServicesBuilder(services);
             return builder;
         }
         
         /// <summary>
         /// Adds Firestorm services.
         /// </summary>
         public static IServiceCollection AddFirestorm(this IServiceCollection services, Action<IFirestormServicesBuilder> configureAction)
         {
             IFirestormServicesBuilder builder = new AspNetCoreServicesBuilder(services);
             configureAction(builder);
             return services;
         }
     }
 }