using System;
using System.Threading.Tasks;
using Firestorm.Core;

namespace Firestorm.Tests.Endpoints.Models
{
    public class MemoryRestScalar : IRestScalar
    {
        private readonly object _value;

        public MemoryRestScalar(object value)
        {
            _value = value;
        }

        public Task<object> GetAsync()
        {
            return Task.FromResult(_value);
        }

        public Task<Acknowledgment> EditAsync(object value)
        {
            throw new NotImplementedException("Not implemented editing test memory scalars yet.");
        }
    }
}