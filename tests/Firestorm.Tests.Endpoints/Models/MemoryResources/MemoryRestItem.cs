using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Firestorm.Core;

namespace Firestorm.Tests.Endpoints.Models
{
    public class MemoryRestItem<TEntity> : IRestItem
    {
        private readonly TEntity _entity;

        public MemoryRestItem(TEntity entity)
        {
            _entity = entity;
        }

        public IRestResource GetField(string fieldName)
        {
            object value = GetValue(fieldName);
            return new MemoryRestScalar(value);
        }

        public Task<RestItemData> GetDataAsync(IEnumerable<string> fieldNames)
        {
            var returnData = new RestItemData();
            
                foreach (string fieldName in fieldNames ?? GetDefaultFieldNames())
                {
                    object value = GetValue(fieldName);
                    returnData.Add(fieldName, value);
                }

            return Task.FromResult(returnData);
        }

        public Task<Acknowledgment> EditAsync(RestItemData itemData)
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgment> DeleteAsync()
        {
            throw new NotImplementedException();
        }

        const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

        private static IEnumerable<string> GetDefaultFieldNames()
        {
            return typeof(TEntity).GetProperties(BINDING_FLAGS).Select(pi => pi.Name);
        }

        private object GetValue(string fieldName)
        {
            var property = typeof(TEntity).GetProperty(fieldName, BINDING_FLAGS);

            if(property == null)
                throw new ArgumentException("A field does not exist with this name.", nameof(fieldName));

            var value = property.GetValue(_entity);
            return value;
        }
    }
}