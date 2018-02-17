using Firestorm.Endpoints;
using Firestorm.Endpoints.Naming;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Start;
using Firestorm.Endpoints.Web;
using Firestorm.EntityFramework6;
using Firestorm.Stems.AutoMap;
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
                EndpointConfiguration = new DefaultRestEndpointConfiguration
                {
                    ResponseConfiguration = new ResponseConfiguration
                    {
                        StatusField = ResponseStatusField.StatusCode,
                        ShowDeveloperErrors = true
                    },
                    NamingConventionSwitcher = new DefaultNamingConventionSwitcher(new NamingConventionOptions
                    {
                        TwoLetterAcronyms = new[] { "ID" }
                    }),
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
                        AutoPropertyMapper = new DefaultPropertyAutoMapper()
                    }
                }
            };
        }
    }
}