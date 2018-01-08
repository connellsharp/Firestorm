using System.Linq;
using Firestorm.Fluent;
using Firestorm.Tests.Examples.Football.Models;

namespace Firestorm.Tests.Examples.Football.Web
{
    public class FootballRestContext : RestContext
    {
        protected override void OnApiCreating(IApiBuilder apiBuilder)
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
                            Wins = t.Fixtures.Count(f => f.Fixture.Goals.Count(g => g.Player.Team == t) > f.Fixture.Goals.Count(g => g.Player.Team == t)),
                            Draws = t.Fixtures.Count(f => f.Fixture.Goals.Count(g => g.Player.Team == t) == f.Fixture.Goals.Count(g => g.Player.Team == t)),
                            Losses = t.Fixtures.Count(f => f.Fixture.Goals.Count(g => g.Player.Team == t) < f.Fixture.Goals.Count(g => g.Player.Team == t)),
                        })
                        .Select(t => new TeamPosition
                        {
                            Points = (t.Wins * 3) + (t.Draws * 1),
                            Team = t.Team.Name
                        }))
                    .HasName("teams")
                    .IsCollection(tp =>
                    {
                        tp.Field(l => l.Points).HasName("points");
                        tp.Field(l => l.Team).HasName("team");
                    });
            });

            apiBuilder.Item<Team>(e =>
            {
                e.RootName = "teams";

                e.Field(t => t.Name)
                    .HasName("name");

                e.Field(t => t.FoundedYear)
                    .HasName("founded");

                e.Field(t => t.Players)
                    .HasName("players")
                    .IsCollection();

                e.Field(t => t.Fixtures)
                    .HasName("fixtures")
                    .IsCollection(c =>
                    {
                        c.Identifier(f => f.FixtureId);

                        c.Field(f => f.IsHome).HasName("home");

                        c.Field(f => f.Fixture.Teams).HasName("teams").IsCollection(t =>
                        {
                            t.Identifier(tt => tt.Team.Id);
                            t.Field(tt => tt.IsHome).HasName("home");
                        });
                    });
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
                    .HasName("goals")
                    .IsCollection(g =>
                    {
                        g.Field(h => h.Id).HasName("id");
                    });

                e.Field(p => p.Team)
                    .HasName("team")
                    .IsItem();
            });
        }
    }

    public class TeamPosition
    {
        public int Points { get; set; }
        public string Team { get; set; }
    }
}