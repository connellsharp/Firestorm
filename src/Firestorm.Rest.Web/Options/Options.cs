using System.Collections.Generic;

namespace Firestorm.Rest.Web.Options
{
    public class Options
    {
        public string Description { get; set; }

        public List<OptionsMethod> AllowedMethods { get; set; }

        public List<RestResourceInfo> SubResources { get; set; }
    }
}