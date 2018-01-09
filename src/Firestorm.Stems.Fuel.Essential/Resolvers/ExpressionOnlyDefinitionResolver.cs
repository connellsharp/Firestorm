using System;
using System.Diagnostics;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Additives.Writers;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Handlers;
using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using Firestorm.Stems.Fuel.Resolving.Factories;

namespace Firestorm.Stems.Fuel.Essential.Resolvers
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

            // TODO: refactor below

            if (FieldDefinition.Getter.Expression != null)
            {
                Type readerPropertyValueType = ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(FieldDefinition.Getter.Expression.GetType());
                Type readerFieldValueType = typeof(ExpressionFieldReader<,>).MakeGenericType(typeof(TItem), readerPropertyValueType); // PropertyExpressionFieldReader ?
                var reader = (IFieldReader<TItem>) Activator.CreateInstance(readerFieldValueType, FieldDefinition.Getter.Expression);
                var readerFactory = new SingletonFactory<IFieldReader<TItem>, TItem>(reader);
                implementations.ReaderFactories.Add(FieldDefinition.FieldName, readerFactory);
            }

            if (FieldDefinition.Setter.Expression != null)
            {
                Type writerPropertyValueType = ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(FieldDefinition.Setter.Expression.GetType());
                Type writerFieldValueType = typeof(PropertyExpressionFieldWriter<,>).MakeGenericType(typeof(TItem), writerPropertyValueType);
                var writer = (IFieldWriter<TItem>) Activator.CreateInstance(writerFieldValueType, FieldDefinition.Setter.Expression);
                var writerFactory = new SingletonFactory<IFieldWriter<TItem>, TItem>(writer);
                implementations.WriterFactories.Add(FieldDefinition.FieldName, writerFactory);
            }

            if (FieldDefinition.Locator.Expression != null)
            {
                Type locatorPropertyValueType = ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(FieldDefinition.Locator.Expression.GetType());
                Debug.Assert(locatorPropertyValueType == FieldDefinition.FieldType, "FieldType is incorrect");
                Type locatorFieldValueType = typeof(IdentifierExpressionItemLocator<,>).MakeGenericType(typeof(TItem), locatorPropertyValueType);
                var locator = (IItemLocator<TItem>) Activator.CreateInstance(locatorFieldValueType, FieldDefinition.Locator.Expression);
                var locatorFactory = new SingletonFactory<IItemLocator<TItem>, TItem>(locator);
                implementations.LocatorFactories.Add(FieldDefinition.FieldName, locatorFactory);
            }
        }
    }
}