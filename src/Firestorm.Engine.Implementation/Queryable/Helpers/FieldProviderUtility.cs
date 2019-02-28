using System;
using System.Collections.Generic;
using System.Linq;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine.Queryable
{
    // TODO consider if this should this be in the engine? Only used from outside, but uses internal members
    // especially now we  MemberInitExpressionBuilder is its own class. Perhaps that could take constructor overloads?
    public static class FieldProviderUtility
    {
        // some similarities here with QueryableFieldSelectorBase

        public static Type GetDynamicType<TItem>(IFieldProvider<TItem> fieldProvider)
            where TItem : class
        {
            return GetDynamicType(fieldProvider, 1);
        }

        public static Type GetDynamicType<TItem>(IFieldProvider<TItem> fieldProvider, int nestedBy)
            where TItem : class
        {
            return GetDynamicType(fieldProvider, fieldProvider.GetDefaultNames(nestedBy));
        }

        public static Type GetDynamicType<TItem>(IFieldProvider<TItem> fieldProvider, IEnumerable<string> fields)
            where TItem : class
        {
            var dynamicFields = fields.Select(f => new KeyValuePair<string, Type>(f, fieldProvider.GetReader(f).FieldType));
            Type dynamicType = LinqRuntimeTypeBuilder.GetDynamicType(dynamicFields);
            return dynamicType;
        }

        public static IDictionary<string, IFieldReader<TItem>> GetReaders<TItem>(this IFieldProvider<TItem> fieldProvider, int nestedBy)
            where TItem : class
        {
            return fieldProvider.GetReaders(fieldProvider.GetDefaultNames(nestedBy));
        }

        public static IDictionary<string, IFieldReader<TItem>> GetReaders<TItem>(this IFieldProvider<TItem> fieldProvider, [NotNull] IEnumerable<string> fieldNames)
            where TItem : class
        {
            if (fieldProvider == null) throw new ArgumentNullException(nameof(fieldProvider));
            if (fieldNames == null) throw new ArgumentNullException(nameof(fieldNames));

            var dict = new Dictionary<string, IFieldReader<TItem>>();

            foreach (string fieldName in fieldNames)
            {
                if (!fieldProvider.FieldExists(fieldName))
                    throw new FieldNotFoundException(fieldName, false);

                IFieldReader<TItem> reader = fieldProvider.GetReader(fieldName);
                if (reader == null)
                    throw new FieldOperationNotAllowedException(fieldName, FieldOperation.Read);

                dict.Add(fieldName, reader);
            }

            return dict;
        }
    }
}
