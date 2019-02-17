using System;
using System.Diagnostics;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Additives.Writers;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using Firestorm.Stems.Fuel.Resolving.Factories;
using Reflectious;

namespace Firestorm.Stems.Essentials.Factories.Resolvers
{
    /// <summary>
    /// Resolves reusable expression readers for Stem static expression properties.
    /// </summary>
    internal class ExpressionOnlyDefinitionAnalyzer : IFieldDefinitionAnalyzer
    {
        public void Analyze<TItem>(EngineImplementations<TItem> implementations, FieldDefinition definition) 
            where TItem : class
        {
            if (definition.SubstemType != null)
                return; // handled by the SubstemDefinitionResolver instead

            if (definition.Getter.GetInstanceMethod != null)
                return; // handled by the RuntimeMethodDefinitionResolver instead

            if (definition.Getter.Expression != null)
            {
                try
                {
                    IncludeGetterExpression(implementations, definition);
                }
                catch (Exception ex)
                {
                    throw new StemAttributeSetupException("Unable create a reader from getter expression "
                                                          + definition.FieldName, ex);
                }
            }

            if (definition.Setter.Expression != null)
            {
                try
                {
                    IncludeSetterExpression(implementations, definition);
                }
                catch (Exception ex)
                {
                    throw new StemAttributeSetupException("Unable create a writer from setter expression "
                                                          + definition.FieldName, ex);
                }
            }

            if (definition.Locator.Expression != null)
            {
                try
                {
                    IncludeLocatorExpression(implementations, definition);
                }
                catch (Exception ex)
                {
                    throw new StemAttributeSetupException("Unable create a locator from expression "
                                                          + definition.FieldName, ex);
                }
            }
        }

        // TODO: refactor below - use Expression.ReturnType?

        private static void IncludeGetterExpression<TItem>(EngineImplementations<TItem> implementations, FieldDefinition definition) 
            where TItem : class
        {
            Type readerPropertyValueType =
                ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(
                    definition.Getter.Expression.GetType());

            var reader = Reflect.Type(typeof(ExpressionFieldReader<,>))
                .MakeGeneric(typeof(TItem), readerPropertyValueType)
                .CastTo<IFieldReader<TItem>>()
                .CreateInstance(definition.Getter.Expression);

            var readerFactory = new SingletonFactory<IFieldReader<TItem>, TItem>(reader);
            implementations.ReaderFactories.Add(definition.FieldName, readerFactory);

            var collatorFactory =
                new SingletonFactory<IFieldCollator<TItem>, TItem>(new BasicFieldCollator<TItem>(reader));
            implementations.CollatorFactories.Add(definition.FieldName, collatorFactory);
        }

        private static void IncludeSetterExpression<TItem>(EngineImplementations<TItem> implementations, FieldDefinition definition)
            where TItem : class
        {
            Type writerPropertyValueType =
                ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(definition.Setter.Expression.GetType());

            var writer = Reflect.Type(typeof(PropertyExpressionFieldWriter<,>))
                .MakeGeneric(typeof(TItem), writerPropertyValueType)
                .CastTo<IFieldWriter<TItem>>()
                .CreateInstance(definition.Setter.Expression);

            var writerFactory = new SingletonFactory<IFieldWriter<TItem>, TItem>(writer);
            implementations.WriterFactories.Add(definition.FieldName, writerFactory);
        }

        private static void IncludeLocatorExpression<TItem>(EngineImplementations<TItem> implementations, FieldDefinition definition)
            where TItem : class
        {
            Type locatorPropertyValueType =
                ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(definition.Locator.Expression.GetType());
            
            Debug.Assert(locatorPropertyValueType == definition.FieldType, "FieldType is incorrect");

            var locator = Reflect.Type(typeof(IdentifierExpressionItemLocator<,>))
                .MakeGeneric(typeof(TItem), locatorPropertyValueType)
                .CastTo<IItemLocator<TItem>>()
                .CreateInstance(definition.Locator.Expression);

            var locatorFactory = new SingletonFactory<IItemLocator<TItem>, TItem>(locator);
            implementations.LocatorFactories.Add(definition.FieldName, locatorFactory);
        }
    }
}