using System.Collections.Generic;

namespace Firestorm.Core.Web.Options
{
    public class Options
    {
        public string Description { get; set; }

        public List<OptionsMethod> AllowedMethods { get; set; }

        public List<RestResourceInfo> SubResources { get; set; }
    }

    public class OptionsMethod
    {
        public string Verb { get; set; }

        public string Description { get; set; }
    }
}