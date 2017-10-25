using System;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Fuel.Essential.Factories;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using Firestorm.Stems.Fuel.Resolving.Factories;

namespace Firestorm.Stems.Fuel.Essential.Resolvers
{
    /// <summary>
    /// Resolves factories using delegates created from methods in the Stem classes.
    /// </summary>
    internal class RuntimeMethodDefinitionResolver : IFieldDefinitionResolver
    {
        public IStemConfiguration Configuration { get; set; }
        public FieldDefinition FieldDefinition { get; set; }

        public void IncludeDefinition<TItem>(EngineImplementations<TItem> implementations)
            where TItem : class
        {
            if (FieldDefinition.SubstemType != null)
                return; // handled by the SubstemDefinitionResolver instead

            if (FieldDefinition.Getter.GetInstanceMethod != null)
            {
                if (FieldDefinition.Getter.Expression != null)
                {
                    var middleExpression = FieldDefinition.Getter.Expression;
                    Type getterType = typeof(InstanceMethodWithExpressionFieldReaderFactory<,,>).MakeGenericType(typeof(TItem), middleExpression.ReturnType,
                        FieldDefinition.FieldType);
                    var getter = (IFactory<IFieldReader<TItem>, TItem>) Activator.CreateInstance(getterType, middleExpression, FieldDefinition.Getter.GetInstanceMethod);
                    implementations.ReaderFactories.Add(FieldDefinition.FieldName, getter);
                }
                else
                {
                    Type getterType = typeof(InstanceMethodFieldReaderFactory<,>).MakeGenericType(typeof(TItem), FieldDefinition.FieldType);
                    var getter = (IFactory<IFieldReader<TItem>, TItem>) Activator.CreateInstance(getterType, FieldDefinition.Getter.GetInstanceMethod);
                    implementations.ReaderFactories.Add(FieldDefinition.FieldName, getter);
                }

            }

            if (FieldDefinition.Setter.GetInstanceMethod != null)
            {
                Type setterType = typeof(ActionFieldWriterFactory<,>).MakeGenericType(typeof(TItem), FieldDefinition.FieldType);
                var setter = (IFactory<IFieldWriter<TItem>, TItem>) Activator.CreateInstance(setterType, FieldDefinition.Setter.GetInstanceMethod);
                implementations.WriterFactories.Add(FieldDefinition.FieldName, setter);
            }

            if (FieldDefinition.Locator.GetInstanceMethod != null)
            {
                var locatorFactory = new DelegateItemLocatorFactory<TItem>(FieldDefinition.Locator.GetInstanceMethod);
                implementations.LocatorFactories.Add(FieldDefinition.FieldName, locatorFactory);
            }
        }
    }
}