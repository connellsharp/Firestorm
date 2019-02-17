using Firestorm.Features;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Essentials;

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
            services.DefinitionAnalyzers = new DefaultFieldDefinitionAnalyzers();
        }
    }
}