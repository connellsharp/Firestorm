using System.Collections.Generic;
using System.Linq;

namespace Firestorm
{
    public class RestDirectoryInfo
    {
        public RestDirectoryInfo(IEnumerable<RestResourceInfo> resourceInfos)
        {
            Resources = resourceInfos;
        }

        public IEnumerable<RestResourceInfo> Resources { get; }
    }

    public class RestResourceInfo
    {
        public RestResourceInfo(string name, ResourceType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }

        public ResourceType Type { get; }

        public string Description { get; set; }
    }
}