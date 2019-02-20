using Firestorm.Stems.Analysis;
using Firestorm.Stems.AutoMap;

namespace Firestorm.Stems.Tests
{
    internal class TestStemsServices : StemsServices
    {
        public TestStemsServices()
        {
            AutoPropertyMapper = new DefaultPropertyAutoMapper();
            ServiceGroup = new DefaultServiceGroup(AutoPropertyMapper);
        }
    }
}