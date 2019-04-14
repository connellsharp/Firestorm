using System;
using System.Collections.Generic;
using Firestorm.Data;
using Firestorm.Engine.Subs;
using Firestorm.Engine.Subs.Wrapping;
using FluentValidation;

namespace Firestorm.FluentValidation
{
    internal class FluentValidationDataSource : IDataSource
    {
        private readonly IDataSource _dataSource;
        private readonly IValidatorProvider _validatorProvider;

        public FluentValidationDataSource(IDataSource dataSource, IValidatorProvider validatorProvider)
        {
            _dataSource = dataSource;
            _validatorProvider = validatorProvider;
        }

        public IEnumerable<Type> FindEntityTypes()
        {
            return _dataSource.FindEntityTypes();
        }

        public IDataContext<TEntity> CreateContext<TEntity>()
            where TEntity : class, new()
        {
            var dataContext = _dataSource.CreateContext<TEntity>();
            IValidator<TEntity> validator = _validatorProvider.GetValidator<TEntity>();
            IDataChangeEvents<TEntity> dataChangeEvents = new FluentValidationDataChangeEvents<TEntity>(validator);
            return new EventTriggeringDataContext<TEntity>(dataContext, dataChangeEvents);
        }
    }
}