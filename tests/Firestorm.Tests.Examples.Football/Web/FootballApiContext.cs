using Firestorm.Fluent;
using Firestorm.Tests.Examples.Football.Models;

namespace Firestorm.Tests.Examples.Football.Web
{
    public class FootballApiContext : ApiContext
    {
        protected override void OnModelCreating(IApiBuilder apiBuilder)
        {
            apiBuilder.Item<Team>(e =>
            {
                e.Field(t => t.Name);

                e.Field(t => t.FoundedYear);

                //e.Field(t => t.Players);
            });

            apiBuilder.Item<Player>(e =>
            {
                e.RootName = "players";

                e.Field(p => p.Name)
                    .HasName("name");

                e.Field(p => p.SquadNumber)
                    .HasName("number");
            });
        }
    }
}
