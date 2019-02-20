using Firestorm.Rest.Web;

namespace Firestorm.Endpoints
{
    public interface IPageLinkCalculator
    {
        PageLinks Calculate(PageInstruction instruction, PageDetails details);
    }
}