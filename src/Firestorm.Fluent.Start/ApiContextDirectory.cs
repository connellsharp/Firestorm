using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Endpoints;
using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent.Start
{
    public class ApiContextDirectory : IRestDirectory
    {
        private readonly IApiDirectorySource _directorySource;

        internal ApiContextDirectory(IRestEndpointContext apiContext, IApiDirectorySource directorySource)
        {
            _directorySource = directorySource;
        }

        public IRestResource GetChild(string startResourceName)
        {
            IRestCollectionSource source = _directorySource.GetCollectionSource(startResourceName);

            return source?.GetRestCollection();
        }

        public Task<RestDirectoryInfo> GetInfoAsync()
        {
            IEnumerable<RestResourceInfo> restResourceInfos = _directorySource.GetCollectionNames().Select(name => new RestResourceInfo(name, ResourceType.Collection));
            RestDirectoryInfo directoryInfo = new RestDirectoryInfo(restResourceInfos);
            return Task.FromResult(directoryInfo);
        }
    }
}