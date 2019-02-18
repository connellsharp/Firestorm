using Firestorm.Stems.AutoMap;

namespace Firestorm.Stems
{
    public class StemsServices : IStemsCoreServices
    {
        public IDependencyResolver DependencyResolver { get; set; }

        public IPropertyAutoMapper AutoPropertyMapper { get; set; }
        
        public IServiceGroup ServiceGroup { get; set; }
    }
}