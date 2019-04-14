using Firestorm.Features;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.AutoMap;

namespace Firestorm.Stems
{
    public class DefaultStemsCustomization : ICustomization<StemsServices>
    {
        private readonly IRequestServiceProvider _requestServiceProvider;

        public DefaultStemsCustomization(IRequestServiceProvider requestServiceProvider)
        {
            _requestServiceProvider = requestServiceProvider;
        }
        
        public StemsServices Apply(StemsServices services)
        {
            services.DependencyResolver = new DefaultDependencyResolver(_requestServiceProvider);
            services.AutoPropertyMapper = new DefaultPropertyAutoMapper();
            services.ServiceGroup = new DefaultServiceGroup(services.AutoPropertyMapper);

            return services;
        }
    }
}