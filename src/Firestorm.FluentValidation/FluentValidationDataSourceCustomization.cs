using System;
using Firestorm.Data;
using Firestorm.Features;

namespace Firestorm.FluentValidation
{
    public class FluentValidationDataSourceCustomization : ICustomization<IDataSource>
    {
        private readonly IValidatorProvider _serviceProvider;

        public FluentValidationDataSourceCustomization(IServiceProvider serviceProvider)
        {
            _serviceProvider = new ServiceProviderValidatorProvider(serviceProvider);
        }

        public IDataSource Apply(IDataSource dataSource)
        {
            return new FluentValidationDataSource(dataSource, _serviceProvider);
        }
    }
}