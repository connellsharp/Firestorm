using System.Diagnostics.CodeAnalysis;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Authorization;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Defaults;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using Firestorm.Testing.Models;

namespace Firestorm.Testing.Http.Models
{
    // TODO exact same file in Firestorm.Engine.Tests
    [ExcludeFromCodeCoverage]
    public class CodedArtistEntityContext : IEngineContext<Artist>
    {
        public CodedArtistEntityContext(IRestUser user)
        {
            Transaction = new VoidTransaction();
            Repository = new ArtistMemoryRepository();
            AuthorizationChecker = new AllowAllAuthorizationChecker<Artist>();
        }

        public IDataTransaction Transaction { get; }

        public IEngineRepository<Artist> Repository { get; }

        public IIdentifierProvider<Artist> Identifiers { get; } = new SingleIdentifierProvider<Artist>(new ExpressionIdentifierInfo<Artist, int>(a => a.ID));

        public IFieldProvider<Artist> Fields => StaticFieldProvider;

        private static IFieldProvider<Artist> StaticFieldProvider { get; } = new FieldDictionary<Artist>
        {
            {"Id", a => a.ID},
            {"Name", a => a.Name}
        };

        public IAuthorizationChecker<Artist> AuthorizationChecker { get; }
    }
}