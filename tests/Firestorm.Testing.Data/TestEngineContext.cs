using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Additives;
using Firestorm.Engine.Additives.Authorization;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Defaults;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using Firestorm.Testing.Models;

namespace Firestorm.Testing.Data
{
    public class TestEngineContext : IEngineContext<Artist>
    {
        public TestEngineContext(IDataTransaction transaction, IEngineRepository<Artist> repository)
        {
            Data = new DataContext<Artist>
            {
                Transaction = transaction,
                Repository = repository,
                AsyncQueryer = new MemoryAsyncQueryer()
            };
        }

        public IDataContext<Artist> Data { get; }
        
        public IIdentifierProvider<Artist> Identifiers { get; } = new SingleIdentifierProvider<Artist>(new ExpressionIdentifierInfo<Artist,int>(a => a.ID));

        public IFieldProvider<Artist> Fields { get; } = new ExpressionListFieldProvider<Artist>
        {
            { "id", a => a.ID },
            { "name", a => a.Name },
        };

        public IAuthorizationChecker<Artist> AuthorizationChecker { get; } = new AllowAllAuthorizationChecker<Artist>();
    }
}