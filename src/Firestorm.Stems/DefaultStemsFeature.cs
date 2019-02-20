using Firestorm.Features;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.AutoMap;

namespace Firestorm.Stems
{
    public class DefaultStemsFeature : IFeature<StemsServices>
    {
        private readonly IRequestServiceProvider _requestServiceProvider;

        public DefaultStemsFeature(IRequestServiceProvider requestServiceProvider)
        {
            _requestServiceProvider = requestServiceProvider;
        }
        
        public void AddTo(StemsServices services)
        {
            services.DependencyResolver = new DefaultDependencyResolver(_requestServiceProvider);
            services.AutoPropertyMapper = new DefaultPropertyAutoMapper();
            services.ServiceGroup = new DefaultServiceGroup(services.AutoPropertyMapper);
        }
    }
}