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
    /// Creates field readers for instance methods coupled with an expression.
    /// The expression is executed as part of the query and the method is executed afterwards in the application.
    /// </summary>
    internal class InstanceMethodWithExpressionFieldReaderFactory<TItem, TMiddle, TValue> : IFactory<IFieldReader<TItem>, TItem>
        where TItem : class
    {
        private readonly Expression<Func<TItem, TMiddle>> _middleExpression;
        private readonly FieldDefinitionHandlerPart.GetInstanceMethodDelegate _getInstanceReaderMethod;

        public InstanceMethodWithExpressionFieldReaderFactory(Expression<Func<TItem, TMiddle>> middleExpression, [NotNull] FieldDefinitionHandlerPart.GetInstanceMethodDelegate getInstanceReaderMethod)
        {
            _middleExpression = middleExpression;
            _getInstanceReaderMethod = getInstanceReaderMethod
                                        ?? throw new ArgumentNullException(nameof(getInstanceReaderMethod));
        }

        public IFieldReader<TItem> Get(Stem<TItem> stem)
        {
            var instanceMethod = _getInstanceReaderMethod.Invoke(stem);

            if (instanceMethod is Func<TMiddle, TValue> delegateMethod)
            {
                return new ExpressionMiddleDelegateFieldReader<TItem, TMiddle, TValue>(_middleExpression, delegateMethod);
            }

            throw new StemAttributeSetupException("Field instance method was invalid.");
        }
    }
}