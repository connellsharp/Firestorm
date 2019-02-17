using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Essentials.Factories.Factories;
using Firestorm.Stems.Fuel.Resolving;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using Firestorm.Stems.Fuel.Resolving.Factories;

namespace Firestorm.Stems.Essentials.Factories.Analyzers
{
    internal class SubstemDefinitionAnalyzer : IDefinitionAnalyzer<FieldDefinition>
    {
        public void Analyze<TItem>(EngineImplementations<TItem> implementations, FieldDefinition definition) 
            where TItem : class
        {
            if (definition.SubstemType == null)
                return;

            LambdaExpression getterExpression = definition.Getter.Expression;

            if (getterExpression == null)
                throw new StemAttributeSetupException("Substem types must be on an expression property.");

            var typeArgs = new HandlerFactoryTypeArguments();
            typeArgs.LoadExpression<TItem>(getterExpression);

            AddFactory(implementations.FullResourceFactories, HandlerTypes.FullResource, typeArgs, definition);
            AddFactory(implementations.ReaderFactories, HandlerTypes.Reader, typeArgs, definition);
            AddFactory(implementations.WriterFactories, HandlerTypes.Writer, typeArgs, definition);
        }

        /// <summary>
        /// Dynamically creates a factory of<see cref="THandler"/>s and adds to the given <see cref="dictionary"/>.
        /// </summary>
        private void AddFactory<THandler, TItem>
        (
            IDictionary<string, IFactory<THandler, TItem>> dictionary,
            HandlerTypes handlerType,
            HandlerFactoryTypeArguments typeArgs,
            FieldDefinition definition
        )
            where THandler : IFieldHandler<TItem>
            where TItem : class
        {
            try
            {
                Type typeDefinition = GetFactoryTypeDefinition(handlerType, typeArgs.IsCollection);
                Type[] typeArguments = GetFactoryTypeArgs<TItem>(typeArgs, definition).ToArray();
                Type factoryType = typeDefinition.MakeGenericType(typeArguments);

                var autoActivator = new AutoActivator(new SubstemHandlerDependencyResolver(typeArgs.GetterExpression, definition));
                
                var factory = (IFactory<THandler, TItem>)autoActivator.CreateInstance(factoryType);

                dictionary.Add(definition.FieldName, factory);
            }
            catch(Exception ex)
            {
                string message = "Cannot add " + handlerType + " factory for this " + (typeArgs.IsCollection ? "collection" : "item");
                throw new StemAttributeSetupException(message, ex);
            }
        }

        private Type GetFactoryTypeDefinition(HandlerTypes handlerType, bool isCollection)
        {
            // There's some weird stuff going on here with generics. I'm not sure it's all necessary..

            switch (handlerType)
            {
                case HandlerTypes.FullResource:
                    return isCollection ? typeof(SubCollectionFieldFullResourceFactory<,,,>) : typeof(SubItemFieldFullResourceFactory<,,>);

                case HandlerTypes.Reader:
                    return isCollection ? typeof(SubCollectionFieldReaderFactory<,,,>) : typeof(SubItemFieldReaderFactory<,,>);

                case HandlerTypes.Writer:
                    return isCollection ? typeof(SubCollectionFieldWriterFactory<,,,>) : typeof(SubItemFieldWriterFactory<,,>);

                default:
                    throw new ArgumentOutOfRangeException(nameof(handlerType), handlerType, null);
            }
        }

        private IEnumerable<Type> GetFactoryTypeArgs<TItem>(HandlerFactoryTypeArguments typeArgs, FieldDefinition definition)
        {
            yield return typeof(TItem);

            yield return typeArgs.PropertyValueType;

            // By convention, all the collection types have 4 generic type parameters.
            // The 3rd parameter is the TNav, because the property type above becomes TCollection.
            if (typeArgs.IsCollection)
                yield return typeArgs.EnumerableTypeArgument;

            yield return definition.SubstemType;
        }

        private class SubstemHandlerDependencyResolver : IDependencyResolver
        {
            private readonly LambdaExpression _getterExpression;
            private readonly FieldDefinition _fieldDefinition;

            public SubstemHandlerDependencyResolver(LambdaExpression getterExpression, FieldDefinition fieldDefinition)
            {
                _getterExpression = getterExpression;
                _fieldDefinition = fieldDefinition;
            }

            public object Resolve(Type type)
            {
                if (typeof(Expression).IsAssignableFrom(type))
                    return _getterExpression;

                if (type == typeof(FieldDefinition))
                    return _fieldDefinition;

                Debug.Fail("One of the handler types listed below is requesting a dependency that is not supported here.");
                return null;
            }
        }

        private enum HandlerTypes
        {
            FullResource,
            Reader,
            Writer
        }
    }
}