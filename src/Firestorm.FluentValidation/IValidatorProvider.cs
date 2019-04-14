using FluentValidation;

namespace Firestorm.FluentValidation
{
    internal interface IValidatorProvider
    {
        IValidator<TEntity> GetValidator<TEntity>();
    }
}