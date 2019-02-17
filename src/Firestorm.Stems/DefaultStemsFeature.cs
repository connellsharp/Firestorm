using Firestorm.Features;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Essentials;

namespace Firestorm.Stems
{
    public class DefaultStemsFeature : IFeature<StemsServices>
    {
        public void AddTo(StemsServices services)
        {
            services.AutoPropertyMapper = new DefaultPropertyAutoMapper();
            services.DefinitionAnalyzers = new DefaultFieldDefinitionAnalyzers();
            services.ImplementationResolver = new ImplementationCache();
        }
    }
}