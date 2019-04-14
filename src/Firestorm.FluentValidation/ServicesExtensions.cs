using Firestorm.Data;
using Firestorm.Features;
using JetBrains.Annotations;

namespace Firestorm.FluentValidation
{
    [PublicAPI]
    public static class ServicesExtensions
    {
        /// <summary>
        /// Configures Firestorm endpoints.
        /// Assumes <see cref="EndpointConfiguration"/> is already configured elsewhere.
        /// </summary>
        public static IServicesBuilder AddFluentValidation(this IServicesBuilder builder)
        {
            builder
                .AddCustomization<IDataSource, FluentValidationDataSourceCustomization>(sp => new FluentValidationDataSourceCustomization(sp));

            return builder;
        }
    }
}