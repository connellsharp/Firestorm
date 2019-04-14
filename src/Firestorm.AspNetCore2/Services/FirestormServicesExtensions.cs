using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.AspNetCore2
 {
     public static class FirestormServicesExtensions
     {
         /// <summary>
         /// Adds Firestorm services.
         /// </summary>
         public static IServicesBuilder AddFirestorm(this IServiceCollection services)
         {
             services.AddRequiredFirestormServices();
             
             IServicesBuilder builder = new AspNetCoreServicesBuilder(services);
             return builder;
         }
         
         /// <summary>
         /// Adds Firestorm services.
         /// </summary>
         public static IServiceCollection AddFirestorm(this IServiceCollection services, Action<IServicesBuilder> configureAction)
         {
             services.AddRequiredFirestormServices();
             
             IServicesBuilder builder = new AspNetCoreServicesBuilder(services);
             configureAction(builder);
             return services;
         }
         
         private static IServiceCollection AddRequiredFirestormServices(this IServiceCollection services)
         {
             services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
             services.AddSingleton<IRequestServiceProvider, RequestServiceProvider>();
             return services;
         }
     }
 }