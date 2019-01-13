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
    internal class ExpressionOnlyDefinitionResolver : IFieldDefinitionResolver
    {
        public IStemConfiguration Configuration { get; set; }
        public FieldDefinition FieldDefinition { get; set; }

        public void IncludeDefinition<TItem>(EngineImplementations<TItem> implementations)
            where TItem : class
        {
            if (FieldDefinition.SubstemType != null)
                return; // handled by the SubstemDefinitionResolver instead

            if (FieldDefinition.Getter.GetInstanceMethod != null)
                return; // handled by the RuntimeMethodDefinitionResolver instead

            if (FieldDefinition.Getter.Expression != null)
            {
                try
                {
                    IncludeGetterExpression(implementations);
                }
                catch (Exception ex)
                {
                    throw new StemAttributeSetupException("Unable create a reader from getter expression "
                                                          + FieldDefinition.FieldName, ex);
                }
            }

            if (FieldDefinition.Setter.Expression != null)
            {
                try
                {
                    IncludeSetterExpression(implementations);
                }
                catch (Exception ex)
                {
                    throw new StemAttributeSetupException("Unable create a writer from setter expression "
                                                          + FieldDefinition.FieldName, ex);
                }
            }

            if (FieldDefinition.Locator.Expression != null)
            {
                try
                {
                    IncludeLocatorExpression(implementations);
                }
                catch (Exception ex)
                {
                    throw new StemAttributeSetupException("Unable create a locator from expression "
                                                          + FieldDefinition.FieldName, ex);
                }
            }
        }

        // TODO: refactor below - use Expression.ReturnType?

        private void IncludeGetterExpression<TItem>(EngineImplementations<TItem> implementations) 
            where TItem : class
        {
            Type readerPropertyValueType =
                ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(
                    FieldDefinition.Getter.Expression.GetType());

            var reader = Reflect.Type(typeof(ExpressionFieldReader<,>))
                .MakeGeneric(typeof(TItem), readerPropertyValueType)
                .CastTo<IFieldReader<TItem>>()
                .CreateInstance(FieldDefinition.Getter.Expression);

            var readerFactory = new SingletonFactory<IFieldReader<TItem>, TItem>(reader);
            implementations.ReaderFactories.Add(FieldDefinition.FieldName, readerFactory);

            var collatorFactory =
                new SingletonFactory<IFieldCollator<TItem>, TItem>(new BasicFieldCollator<TItem>(reader));
            implementations.CollatorFactories.Add(FieldDefinition.FieldName, collatorFactory);
        }

        private void IncludeSetterExpression<TItem>(EngineImplementations<TItem> implementations)
            where TItem : class
        {
            Type writerPropertyValueType =
                ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(FieldDefinition.Setter.Expression.GetType());

            var writer = Reflect.Type(typeof(PropertyExpressionFieldWriter<,>))
                .MakeGeneric(typeof(TItem), writerPropertyValueType)
                .CastTo<IFieldWriter<TItem>>()
                .CreateInstance(FieldDefinition.Setter.Expression);

            var writerFactory = new SingletonFactory<IFieldWriter<TItem>, TItem>(writer);
            implementations.WriterFactories.Add(FieldDefinition.FieldName, writerFactory);
        }

        private void IncludeLocatorExpression<TItem>(EngineImplementations<TItem> implementations)
            where TItem : class
        {
            Type locatorPropertyValueType =
                ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(FieldDefinition.Locator.Expression.GetType());
            Debug.Assert(locatorPropertyValueType == FieldDefinition.FieldType, "FieldType is incorrect");

            var locator = Reflect.Type(typeof(IdentifierExpressionItemLocator<,>))
                .MakeGeneric(typeof(TItem), locatorPropertyValueType)
                .CastTo<IItemLocator<TItem>>()
                .CreateInstance(FieldDefinition.Locator.Expression);

            var locatorFactory = new SingletonFactory<IItemLocator<TItem>, TItem>(locator);
            implementations.LocatorFactories.Add(FieldDefinition.FieldName, locatorFactory);
        }
    }
}