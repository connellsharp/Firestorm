using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firestorm.Endpoints.Directories
{
    public class DictionaryDirectory : IRestDirectory, IEnumerable<KeyValuePair<string, Type>>
    {
        private readonly Dictionary<string, Type> _startEndpointTypes = new Dictionary<string, Type>();

        public IRestResource GetChild(string startResourceName)
        {
            if (!_startEndpointTypes.ContainsKey(startResourceName))
                throw new ChildNotFoundException("The starting startResourceName '" + startResourceName + "' does not exist.");

            Type startType = _startEndpointTypes[startResourceName];

            var startField = Activator.CreateInstance(startType) as IRestResource;
            return startField;
        }

        public Task<RestDirectoryInfo> GetInfoAsync()
        {
            throw new NotImplementedException();
        }

        public void Add<TResource>(string directoryName)
            where TResource : IRestResource
        {
            _startEndpointTypes.Add(directoryName, typeof(TResource));
        }

        public void Add(string directoryName, Type type)
        {
            if (!typeof(IRestResource).IsAssignableFrom(type))
                throw new TypeLoadException("REST API can only map to implemenations of IRestResource.");

            _startEndpointTypes.Add(directoryName, type);
        }

        public IEnumerator<KeyValuePair<string, Type>> GetEnumerator()
        {
            return _startEndpointTypes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class ChildNotFoundException : RestApiException
        {
            public ChildNotFoundException(string message)
                : base(ErrorStatus.NotFound, message)
            { }
        }
    }
}