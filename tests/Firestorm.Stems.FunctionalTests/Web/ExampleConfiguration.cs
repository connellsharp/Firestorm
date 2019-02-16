using Firestorm.Endpoints;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Formatting.Naming;
using Firestorm.Endpoints.Responses;

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
                return new EndpointConfiguration
                {
                    Response = new ResponseConfiguration
                    {
                        StatusField = ResponseStatusField.StatusCode,
                        ShowDeveloperErrors = true
                    },
                    NamingConventions = new NamingConventionConfiguration
                    {
                        TwoLetterAcronyms = new[] { "ID" }
                    }
                };
            }
        }
    }
}