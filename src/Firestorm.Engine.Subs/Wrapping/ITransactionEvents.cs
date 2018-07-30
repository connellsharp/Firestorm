using System.Threading.Tasks;

namespace Firestorm.Engine.Subs.Wrapping
{
    internal interface ITransactionEvents
    {
        Task OnSavingAsync();

        Task OnSavedAsync();
    }
}