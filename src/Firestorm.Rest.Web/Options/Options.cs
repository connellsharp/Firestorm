using System.Collections.Generic;

namespace Firestorm.Rest.Web.Options
{
    public class Options
    {
        public string Description { get; set; }

        public IEnumerable<OptionsMethod> AllowedMethods { get; set; }

        public IEnumerable<RestResourceInfo> SubResources { get; set; }
    }
}