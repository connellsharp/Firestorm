using System.Collections.Generic;
using System.Linq;

namespace Firestorm.Core.Web
{
    public class DirectoryBody : ResourceBody
    {
        public DirectoryBody(RestDirectoryInfo directory)
        {
            Resources = directory.Resources.Select(GetOutputData);
        }

        private RestItemData GetOutputData(RestResourceInfo info)
        {
            // TODO naming conventions? move to endpoints somehow?
            var data = new RestItemData();

            data.Add("name", info.Name);
            data.Add("type", info.Type.ToString().SeparateCamelCase("_", true));

            if (!string.IsNullOrEmpty(info.Description))
                data.Add("description", info.Description);

            return data;
        }

        public IEnumerable<RestItemData> Resources { get; }

        public override ResourceType ResourceType { get; } = ResourceType.Directory;

        public override object GetObject() => Resources;
    }
}