using System;
using System.Collections.Generic;
using System.Linq;

namespace Firestorm.Rest.Web
{
    public class DirectoryBody : ResourceBody
    {
        private readonly Func<string, string> _pathNameSwitcher;

        public DirectoryBody(RestDirectoryInfo directory, Func<string, string> pathNameSwitcher)
        {
            _pathNameSwitcher = pathNameSwitcher;
            Resources = directory.Resources.Select(GetOutputData);
        }

        private RestItemData GetOutputData(RestResourceInfo info)
        {
            var data = new RestItemData();
            
            data.Add("Name", _pathNameSwitcher(info.Name)); // keys switch naming conventions in .Formatting, but values don't
            data.Add("Type", info.Type.ToString().SeparateCamelCase("_", true)); // TODO naming convention? and error types too

            if (!string.IsNullOrEmpty(info.Description))
                data.Add("Description", info.Description);

            return data;
        }

        public IEnumerable<RestItemData> Resources { get; }

        public override ResourceType ResourceType { get; } = ResourceType.Directory;

        public override object GetObject() => Resources;
    }
}