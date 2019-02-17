using Firestorm.Rest.Web;

namespace Firestorm.Endpoints.Configuration
{
    public interface IPageLinkCalculator
    {
        PageLinks Calculate(PageInstruction instruction, PageDetails details);
    }
}