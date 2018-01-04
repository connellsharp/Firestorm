using System.Linq;
using Firestorm.Fluent;
using Firestorm.Tests.Examples.Football.Models;

namespace Firestorm.Tests.Examples.Football.Web
{
    public class FootballApiContext : ApiContext
    {
        protected override void OnModelCreating(IApiBuilder apiBuilder)
        {
            apiBuilder.Item<League>(e =>
            {
                e.RootName = "leagues";

                e.Field(l => l.Name);

                e.Identifier(l => l.Name.Replace(" ", string.Empty).ToLower());

                e.Field(l => l.Teams
                        .Select(t => new
                        {
                            Team = t,
                            Wins = t.HomeFixtures.Concat(t.AwayFixtures).Count(f => f.Goals.Count(g => g.Player.Team == t) > f.Goals.Count(g => g.Player.Team == t)),
                            Draws = t.HomeFixtures.Concat(t.AwayFixtures).Count(f => f.Goals.Count(g => g.Player.Team == t) == f.Goals.Count(g => g.Player.Team == t)),
                            Losses = t.HomeFixtures.Concat(t.AwayFixtures).Count(f => f.Goals.Count(g => g.Player.Team == t) < f.Goals.Count(g => g.Player.Team == t)),
                        })
                        .Select(t => new TeamPosition
                        {
                            Points = (t.Wins * 3) + (t.Draws * 1),
                            Team = t.Team.Name
                        }))
                    .HasName("teams");
            });

            apiBuilder.Item<Team>(e =>
            {
                e.RootName = "teams";

                e.Field(t => t.Name)
                    .HasName("name");

                e.Field(t => t.FoundedYear)
                    .HasName("founded");

                e.Field(t => t.Players)
                    .HasName("players");

                e.Field(t => t.AwayFixtures.Concat(t.HomeFixtures))
                    .HasName("fixtures");
            });

            apiBuilder.Item<Player>(e =>
            {
                e.RootName = "players";

                e.Field(p => p.Name)
                    .AllowWrite()
                    .HasName("name");

                e.Field(p => p.SquadNumber)
                    .HasName("number");

                e.Field(p => p.Goals)
                    .HasName("goals");

                e.Field(p => p.Team)
                    .HasName("team");
            });
        }
    }

    public class TeamPosition
    {
        public int Points { get; set; }
        public string Team { get; set; }
    }
}