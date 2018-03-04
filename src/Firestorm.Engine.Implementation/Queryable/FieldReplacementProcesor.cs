using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Fields;

namespace Firestorm.Engine.Queryable
{
    internal class FieldReplacementProcesor<TItem>
    {
        private readonly IDictionary<string, IFieldReader<TItem>> _fieldReaders;
        private ConcurrentDictionary<string, IFieldValueReplacer<TItem>> _replacerDictionary;

        public FieldReplacementProcesor(IDictionary<string, IFieldReader<TItem>> fieldReaders)
        {
            _fieldReaders = fieldReaders;
        }

        internal Task LoadAllReplacersAsync(IQueryable<TItem> items)
        {
            _replacerDictionary = new ConcurrentDictionary<string, IFieldValueReplacer<TItem>>();
            var tasks = new List<Task>();

            foreach (KeyValuePair<string, IFieldReader<TItem>> fieldReader in _fieldReaders)
            {
                IFieldValueReplacer<TItem> replacer = fieldReader.Value.Replacer;
                if (replacer == null)
                    continue;

                Task preloadTask = replacer.LoadAsync(items).ContinueWith(task =>
                {
                    if (!_replacerDictionary.TryAdd(fieldReader.Key, replacer))
                        throw new InvalidOperationException("Error adding reader replacer to dictionary.");
                });

                tasks.Add(preloadTask);
            }

            return Task.WhenAll(tasks);
        }

        internal async Task<List<object>> ExecuteWithReplacementsAsync(IQueryable dynamicQueryable, ForEachAsyncDelegate<object> forEachAsync)
        {
            var dynamicType = dynamicQueryable.ElementType;
            var returnObjects = new List<object>();
            
            if (dynamicQueryable.IsInMemory())
                await ItemQueryHelper.DefaultForEachAsync(dynamicQueryable.OfType<object>(), AddObjectToList);
            else
                await forEachAsync(dynamicQueryable.AsObjects(), AddObjectToList);

            void AddObjectToList(object dynamicObj)
            {
                ReplaceWithDictionary(dynamicObj, dynamicType);
                returnObjects.Add(dynamicObj);
            }

            return returnObjects;
        }

        internal void ReplaceWithDictionary(object dynamicObj, Type dynamicType)
        {
            foreach (var replacer in _replacerDictionary)
            {
                Debug.Assert(replacer.Value != null, "Field value should not be in the preloaded list if there is no replacer.");

                FieldInfo fieldInfo = dynamicType.GetField(replacer.Key);

                object dbValue = fieldInfo.GetValue(dynamicObj);
                object replacementValue = replacer.Value.GetReplacement(dbValue);

                fieldInfo.SetValue(dynamicObj, replacementValue);
            }
        }
    }
}