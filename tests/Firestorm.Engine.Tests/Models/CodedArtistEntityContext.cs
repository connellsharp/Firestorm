using Firestorm.Data;
using Firestorm.Engine.Additives.Authorization;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Defaults;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using Firestorm.Testing.Engine;
using Firestorm.Testing.Models;

namespace Firestorm.Engine.Tests.Models
{
    // TODO exact same file in Firestorm.Testing.Http
    public class CodedArtistEntityContext : IEngineContext<Artist>
    {
        public CodedArtistEntityContext(IRestUser user)
        {
            Data = new DataContext<Artist>
            {
                Transaction = new VoidTransaction(),
                Repository = new ArtistMemoryRepository(),
                AsyncQueryer = new MemoryAsyncQueryer()
            };
            
            AuthorizationChecker = new AllowAllAuthorizationChecker<Artist>();
        }

        public IDataContext<Artist> Data { get; }
        
        public IIdentifierProvider<Artist> Identifiers { get; } = new SingleIdentifierProvider<Artist>(new ExpressionIdentifierInfo<Artist, int>(a => a.ID));

        public IFieldProvider<Artist> Fields => StaticFieldProvider;

        private static IFieldProvider<Artist> StaticFieldProvider { get; } = new FieldDictionary<Artist>
        {
            {"id", a => a.ID},
            {"name", a => a.Name}
        };

        public IAuthorizationChecker<Artist> AuthorizationChecker { get; }
    }
}