using Firestorm.Endpoints;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Naming;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Web;

namespace Firestorm.Stems.FunctionalTests.Web
{
    internal static class ExampleConfiguration
    {
        /// <summary>
        /// Gets the example endpoint configuration.
        /// </summary>
        public static RestEndpointConfiguration EndpointConfiguration
        {
            get
            {
                return new DefaultRestEndpointConfiguration
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
                };
            }
        }
    }
}