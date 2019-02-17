using System;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Essentials.Factories.Factories;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using Firestorm.Stems.Fuel.Resolving.Factories;
using Reflectious;

namespace Firestorm.Stems.Essentials.Factories.Resolvers
{
    /// <summary>
    /// Resolves factories using delegates created from methods in the Stem classes.
    /// </summary>
    internal class RuntimeMethodDefinitionAnalyzer : IFieldDefinitionAnalyzer
    {
        public void Analyze<TItem>(EngineImplementations<TItem> implementations, FieldDefinition definition) 
            where TItem : class
        {
            if (definition.SubstemType != null)
                return; // handled by the SubstemDefinitionResolver instead

            if (definition.Getter.GetInstanceMethod != null)
            {
                IFactory<IFieldReader<TItem>, TItem> factory;

                if (definition.Getter.Expression != null)
                {
                    var middleExpression = definition.Getter.Expression;

                    factory = Reflect.Type(typeof(InstanceMethodWithExpressionFieldReaderFactory<,,>))
                        .MakeGeneric(typeof(TItem), middleExpression.ReturnType, definition.FieldType)
                        .CastTo<IFactory<IFieldReader<TItem>, TItem>>()
                        .CreateInstance(middleExpression, definition.Getter.GetInstanceMethod);
                }
                else
                {
                    factory = Reflect.Type(typeof(InstanceMethodFieldReaderFactory<,>))
                        .MakeGeneric(typeof(TItem), definition.FieldType)
                        .CastTo<IFactory<IFieldReader<TItem>, TItem>>()
                        .CreateInstance(definition.Getter.GetInstanceMethod);
                }

                implementations.ReaderFactories.Add(definition.FieldName, factory);
                implementations.CollatorFactories.Add(definition.FieldName, new BasicCollatorFactory<TItem>(factory));
            }

            if (definition.Setter.GetInstanceMethod != null)
            {
                Type setterType = typeof(ActionFieldWriterFactory<,>).MakeGenericType(typeof(TItem), definition.FieldType);
                var setter = (IFactory<IFieldWriter<TItem>, TItem>) Activator.CreateInstance(setterType, definition.Setter.GetInstanceMethod);
                implementations.WriterFactories.Add(definition.FieldName, setter);
            }

            if (definition.Locator.GetInstanceMethod != null)
            {
                var locatorFactory = new DelegateItemLocatorFactory<TItem>(definition.Locator.GetInstanceMethod);
                implementations.LocatorFactories.Add(definition.FieldName, locatorFactory);
            }
        }
    }
}