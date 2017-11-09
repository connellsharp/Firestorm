using System.Reflection;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Start;
using Firestorm.Engine.EntityFramework;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Naming;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Tests.Examples.Data;

namespace Firestorm.Tests.Examples.Web
{
    internal static class ExampleConfiguration
    {
        /// <summary>
        /// Gets the example configuration using the public Stem types nested in the given <see cref="TTest"/> type.
        /// </summary>
        public static FirestormConfiguration GetFirestormConfig<TTest>()
        {
            return new FirestormConfiguration
            {
                EndpointConfiguration = new RestEndpointConfiguration
                {
                    ResponseContentGenerator = new StatusCodeResponseContentGenerator() // PagedBodyResponseContentGenerator
                },
                StartResourceFactory = new StemsStartResourceFactory
                {
                    RootResourceFactory = new DataSourceRootResourceFactory
                    {
                        StemTypes = typeof(TTest).GetNestedTypes(BindingFlags.Public),
                        DataSource = new EntitiesDataSource<ExampleDataContext>(),
                    },
                    StemConfiguration = new DefaultStemConfiguration
                    {
                        NamingConventionSwitcher = new DefaultNamingConventionSwitcher(),
                        AutoPropertyMapper = new DefaultPropertyAutoMapper()
                    }
                }
            };
        }
    }
}