using Firestorm.Features;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Essentials;

namespace Firestorm.Stems
{
    public class DefaultStemsFeature : IFeature<StemsServices>
    {
        public void AddTo(StemsServices services)
        {
            services.AutoPropertyMapper = new DefaultPropertyAutoMapper();
            services.Analyzers = new DefaultFieldDefinitionAnalyzers();
            services.ImplementationResolver = new ImplementationCache();
        }
    }
}