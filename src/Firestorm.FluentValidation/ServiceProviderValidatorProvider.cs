using System;
using FluentValidation;

namespace Firestorm.FluentValidation
{
    public class ServiceProviderValidatorProvider : IValidatorProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderValidatorProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IValidator<TEntity> GetValidator<TEntity>()
        {
            return _serviceProvider.GetService<IValidator<TEntity>>();
        }
    }
}