using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Engine;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Attributes;
using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Fuel.Fields;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using Firestorm.Stems.Fuel.Resolving.Factories;
using Firestorm.Stems.Fuel.Substems.Factories;

namespace Firestorm.Stems.Fuel.Substems
{
    internal class SubstemDefinitionResolver : IFieldDefinitionResolver
    {
        public IStemConfiguration Configuration { get; set; }
        public FieldDefinition FieldDefinition { get; set; }

        public void IncludeDefinition<TItem>(EngineImplementations<TItem> implementations)
            where TItem : class
        {
            if (FieldDefinition.SubstemType == null)
                return;

            LambdaExpression getterExpression = FieldDefinition.Getter.Expression;

            if (getterExpression == null)
                throw new StemAttributeSetupException("Substem types must be on an expression property.");

            Type navPropertyType = GetNavPropertyType<TItem>(ref getterExpression);

            var fullFactoryType = typeof(SubstemFieldFullResourceFactory<,,>).MakeGenericType(typeof(TItem), navPropertyType, FieldDefinition.SubstemType);
            var fullFactory = (IFactory<IFieldResourceGetter<TItem>, TItem>)Activator.CreateInstance(fullFactoryType, getterExpression);
            implementations.FullResourceFactories.Add(FieldDefinition.FieldName, fullFactory);
            
            Type readerFactoryType = typeof(SubstemFieldReaderFactory<,,>).MakeGenericType(typeof(TItem), navPropertyType, FieldDefinition.SubstemType);
            var readerFactory = (IFactory<IFieldReader<TItem>, TItem>)Activator.CreateInstance(readerFactoryType, getterExpression);
            implementations.ReaderFactories.Add(FieldDefinition.FieldName, readerFactory);

            var writerFactoryType = typeof(SubstemFieldWriterFactory<,,>).MakeGenericType(typeof(TItem), navPropertyType, FieldDefinition.SubstemType);
            var writerFactory = (IFactory<IFieldWriter<TItem>, TItem>)Activator.CreateInstance(writerFactoryType, getterExpression);
            implementations.WriterFactories.Add(FieldDefinition.FieldName, writerFactory);
        }

        private static Type GetNavPropertyType<TItem>(ref LambdaExpression getterExpression)
        {
            Type propertyValueType = ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(getterExpression.GetType());

            Type navPropertyType = propertyValueType;
            Type enumerableType = GetEnumerableType(navPropertyType);

            if (enumerableType != null)
            {
                navPropertyType = navPropertyType.GetGenericArguments()[0];
                getterExpression = getterExpression.CastBody(enumerableType);
            }

            return navPropertyType;
        }

        private static Type GetEnumerableType(Type navPropertyType)
        {
            if (navPropertyType == typeof(string))
                return null;

            return navPropertyType.GetGenericInterface(typeof(IEnumerable<>));
        }
    }
}