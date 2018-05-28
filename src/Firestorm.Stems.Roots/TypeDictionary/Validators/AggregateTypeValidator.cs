using System;
using System.Collections.Generic;
using System.Linq;

namespace Firestorm.Stems.Roots
{
    public class AggregateTypeValidator : ITypeValidator
    {
        private readonly IEnumerable<ITypeValidator> _validators;

        public AggregateTypeValidator(params ITypeValidator[] validators)
            : this(validators.AsEnumerable())
        { }

        public AggregateTypeValidator(IEnumerable<ITypeValidator> validators)
        {
            _validators = validators;
        }

        public bool IsValidType(Type type)
        {
            return _validators.All(v => v.IsValidType(type));
        }
    }
}