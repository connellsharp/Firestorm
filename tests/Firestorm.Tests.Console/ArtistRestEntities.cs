using System;
using System.Linq.Expressions;

namespace Firestorm.TestConsole
{
    public class ArtistRestEntities : IRestEntityManager<MusicEntities, Artist>
    {
        private MusicEntities _database;

        public Initialize()
        {
            _database = new MyDatabaseContext();
        }

        public DbSet<Artist> Table
        {
            get { return _database.Artists; }
        }

        public Expression<Func<Artist, bool>> AvailableToApiPredicate
        {
            get { return a => !a.IsDeleted; }
        }
    
        public IFieldMappings FieldMappings
        {
            get { return _fieldMappings; }
        }

        private static IFieldMappings _fieldMappings = new SimpleFieldMappings
        {
            { "id", a => a.ArtistID },
            { "name", a => a.Name },
            { "tracks", a => a.Tracks },
        };

        public Dispose()
        {
            _database.Dispose();
        }
    }
}