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

            // TODO: refactor below - use Expression.ReturnType?

            if (FieldDefinition.Getter.Expression != null)
            {
                Type readerPropertyValueType = ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(FieldDefinition.Getter.Expression.GetType());
                
                var reader = Reflect.Type(typeof(ExpressionFieldReader<,>))
                    .MakeGeneric(typeof(TItem), readerPropertyValueType)
                    .CastTo<IFieldReader<TItem>>()
                    .CreateInstance(FieldDefinition.Getter.Expression);
                
                var readerFactory = new SingletonFactory<IFieldReader<TItem>, TItem>(reader);
                implementations.ReaderFactories.Add(FieldDefinition.FieldName, readerFactory);
                
                var collatorFactory = new SingletonFactory<IFieldCollator<TItem>,TItem>(new BasicFieldCollator<TItem>(reader));
                implementations.CollatorFactories.Add(FieldDefinition.FieldName, collatorFactory);
            }

            if (FieldDefinition.Setter.Expression != null)
            {
                Type writerPropertyValueType = ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(FieldDefinition.Setter.Expression.GetType());
                
                var writer = Reflect.Type(typeof(PropertyExpressionFieldWriter<,>))
                    .MakeGeneric(typeof(TItem), writerPropertyValueType)
                    .CastTo<IFieldWriter<TItem>>()
                    .CreateInstance(FieldDefinition.Setter.Expression);
                
                var writerFactory = new SingletonFactory<IFieldWriter<TItem>, TItem>(writer);
                implementations.WriterFactories.Add(FieldDefinition.FieldName, writerFactory);
            }

            if (FieldDefinition.Locator.Expression != null)
            {
                Type locatorPropertyValueType = ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(FieldDefinition.Locator.Expression.GetType());
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
}