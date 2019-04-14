using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;
using FluentValidation;

namespace Firestorm.FluentValidation
{
    public class ValidatingRepository<T> : IEngineRepository<T> 
        where T : class
    {
        private readonly IEngineRepository<T> _underlyingRepository;
        private readonly IValidator<T> _validator;

        public ValidatingRepository(IEngineRepository<T> underlyingRepository, IValidator<T> validator)
        {
            _underlyingRepository = underlyingRepository;
            _validator = validator;
        }

        public Task InitializeAsync()
        {
            return _underlyingRepository.InitializeAsync();
        }

        public IQueryable<T> GetAllItems()
        {
            return _underlyingRepository.GetAllItems();
        }

        public T CreateAndAttachItem()
        {
            return _underlyingRepository.CreateAndAttachItem();
        }

        public void MarkUpdated(T item)
        {
            _validator.ValidateAndThrow(item);
            _underlyingRepository.MarkUpdated(item);
        }

        public void MarkDeleted(T item)
        {
            _underlyingRepository.MarkDeleted(item);
        }
    }
}