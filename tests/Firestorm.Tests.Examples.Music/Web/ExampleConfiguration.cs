using Firestorm.Endpoints;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Start;
using Firestorm.EntityFramework6;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Naming;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Tests.Examples.Music.Data;

namespace Firestorm.Tests.Examples.Music.Web
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
                    ResponseConfiguration = new ResponseConfiguruation
                    {
                        StatusField = ResponseStatusField.StatusCode
                    }
                },
                StartResourceFactory = new StemsStartResourceFactory
                {
                    RootResourceFactory = new DataSourceRootResourceFactory
                    {
                        StemTypeGetter = new NestedTypeGetter(typeof(TTest)),
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