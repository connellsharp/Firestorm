using Firestorm.Engine;
using Firestorm.Engine.Subs.Context;
using Firestorm.Stems.Fuel;

namespace Firestorm.Stems.Roots.Derive
{
    internal class RootCollectionCreator<TItem> : IRootCollectionCreator
        where TItem : class
    {
        IRestCollection IRootCollectionCreator.GetRestCollection(Root root, Stem stem)
        {
            return GetRestCollection((Root<TItem>) root, (Stem<TItem>) stem);
        }

        public IRestCollection GetRestCollection(Root<TItem> root, Stem<TItem> stem)
        {
            var transaction = new RootDataTransaction(root);
            var repository = new RootEngineRepository<TItem>(root, stem);
            var stemEngineInfo = new StemsEngineSubContext<TItem>(stem);

            var context = new AdditiveEngineContext<TItem>(transaction, repository, stemEngineInfo);

            return new EngineRestCollection<TItem>(context);
        }
    }
}