using System.Threading.Tasks;
using Firestorm.Engine.Subs;
using FluentValidation;

namespace Firestorm.FluentValidation
{
    public class FluentValidationDataChangeEvents<TEntity> : IDataChangeEvents<TEntity>
    {
        private readonly IValidator<TEntity> _validator;

        public FluentValidationDataChangeEvents(IValidator<TEntity> validator)
        {
            _validator = validator;
        }

        public bool HasAnyEvent { get; } = true;
        
        public void OnCreating(TEntity newItem)
        {
            ValidateAndThrow(newItem);
        }

        public void OnUpdating(TEntity item)
        {
            ValidateAndThrow(item);
        }

        public DeletingResult OnDeleting(TEntity item)
        {
            return DeletingResult.Continue;
        }

        public Task OnSavingAsync(TEntity item)
        {
            return Task.FromResult(false);
        }

        public Task OnSavedAsync(TEntity item)
        {
            return Task.FromResult(false);
        }

        private void ValidateAndThrow(TEntity entity)
        {
            var validationResult = _validator.Validate(entity);

            if (!validationResult.IsValid)
                throw new ValidationRestApiException(validationResult.Errors);
        }
    }
}