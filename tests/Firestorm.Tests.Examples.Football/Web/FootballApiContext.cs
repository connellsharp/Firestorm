using System.Collections.Generic;
using System.Linq;
using Firestorm.Fluent;
using Firestorm.Fluent.Sources;
using Firestorm.Tests.Examples.Football.Models;

namespace Firestorm.Tests.Examples.Football.Web
{
    public class FootballApiContext : ApiContext
    {
        public ApiRoot<Team> Teams { get; set; }
        public ApiRoot<Player> Players { get; set; }
        public ApiRoot<Goal> Goals { get; set; }
        public ApiRoot<Fixture> Fixtures { get; set; }
        public ApiRoot<League> Leagues { get; set; }

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
                    .IsCollection(ft =>
                    {
                        ft.Identifier(f => f.FixtureId);

                        ft.Field(f => f.IsHome)
                            .AllowWrite((f, v) =>
                            {
                                f.IsHome = v;

                                var vsTeam = f.Fixture.Teams.FirstOrDefault(tt => tt.TeamId != f.TeamId);
                                if(vsTeam != null)
                                    vsTeam.IsHome = !v;
                            })
                            .HasName("home");

                        ft.Field(f => f.Fixture.Teams.FirstOrDefault(tt => tt.TeamId != f.TeamId))
                            .AllowWrite((f, tt) => { f.Fixture.Teams.Add(tt); })
                            .HasName("vs_team")
                            .IsItem(t =>
                            {
                                t.Field(tt => tt.TeamId)
                                    .HasName("id")
                                    .AllowLocate()
                                    .AllowWrite();
                            });

                        ft.Field(f => f.Fixture.Goals)
                            .HasName("goals")
                            .IsCollection(g => { });

                        ft.OnCreating(f =>
                        {
                            f.Fixture = new Fixture
                            {
                                Teams = new List<FixtureTeam>(2)
                            };
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
                    .IsCollection(g => { g.Field(h => h.Id).HasName("id"); });

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