using System;
using System.Linq.Expressions;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using JetBrains.Annotations;

namespace Firestorm.Stems.Essentials.Factories.Factories
{
    /// <summary>
    /// Field reader for instance methods that return an expression.
    /// Works with instance expressions, which are often used to return different expressions depending on the User.
    /// Or with instance methods that take the item as a parameter.
    /// </summary>
    internal class InstanceMethodFieldReaderFactory<TItem, TValue> : IFactory<IFieldReader<TItem>, TItem>
        where TItem : class
    {
        private readonly FieldDefinitionHandlerPart.GetInstanceMethodDelegate _getInstanceLocatorMethod;

        public InstanceMethodFieldReaderFactory([NotNull] FieldDefinitionHandlerPart.GetInstanceMethodDelegate getInstanceLocatorMethod)
        {
            _getInstanceLocatorMethod = getInstanceLocatorMethod
                                        ?? throw new ArgumentNullException(nameof(getInstanceLocatorMethod));
        }

        public IFieldReader<TItem> Get(Stem<TItem> stem)
        {
            var instanceMethod = _getInstanceLocatorMethod.Invoke(stem);

            if (instanceMethod is Func<Expression<Func<TItem, TValue>>> expressionMethod)
            {
                Expression<Func<TItem, TValue>> expression = expressionMethod.Invoke();
                return new ExpressionFieldReader<TItem, TValue>(expression);
            }

            if (instanceMethod is Func<TItem, TValue> delegateMethod)
            {
                return new DelegateFieldReader<TItem, TValue>(delegateMethod);
            }

            throw new StemAttributeSetupException("Field instance method was invalid.");
        }
    }
}