using Firestorm.Endpoints;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Formatting.Naming;

namespace Firestorm.Stems.FunctionalTests.Web
{
    internal static class ExampleConfiguration
    {
        /// <summary>
        /// Gets the example endpoint configuration.
        /// </summary>
        public static EndpointConfiguration EndpointConfiguration
        {
            get
            {
                return new DefaultEndpointConfiguration
                {
                    Response = new ResponseConfiguration
                    {
                        StatusField = ResponseStatusField.StatusCode,
                        ShowDeveloperErrors = true
                    },
                    NamingConventionSwitcher = new DefaultNamingConventionSwitcher(new NamingConventionOptions
                    {
                        TwoLetterAcronyms = new[] { "ID" }
                    }),
                };
            }
        }
    }
}