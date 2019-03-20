using Firestorm.Stems.AutoMap;

namespace Firestorm.Stems
{
    /// <summary>
    /// Contains the configuration for how the application's Stem objects can be utilised.
    /// </summary>
    public class StemsServices : IStemsCoreServices
    {
        public IDependencyResolver DependencyResolver { get; set; }

        public IPropertyAutoMapper AutoPropertyMapper { get; set; }
        
        public IServiceGroup ServiceGroup { get; set; }
    }
}