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
        //public ApiRoot<League> Leagues { get; set; }

        protected override void OnApiCreating(IApiBuilder apiBuilder)
        {
            apiBuilder.Item<League>(e =>
            {
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
                    .HasName("Teams")
                    .IsCollection(tp =>
                    {
                        tp.Field(l => l.Points);
                        tp.Field(l => l.Team);
                    });
            });

            apiBuilder.Item<Team>(e =>
            {
                e.Field(t => t.Name);

                e.Field(t => t.FoundedYear)
                    .HasName("Founded");

                e.Field(t => t.Players)
                    .IsCollection();

                e.Field(t => t.Fixtures)
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
                            .HasName("Home");

                        ft.Field(f => f.Fixture.Teams.FirstOrDefault(tt => tt.TeamId != f.TeamId))
                            .AllowWrite((f, tt) => { f.Fixture.Teams.Add(tt); })
                            .HasName("VsTeam")
                            .IsItem(t =>
                            {
                                t.Field(tt => tt.TeamId)
                                    .HasName("Id")
                                    .AllowLocate()
                                    .AllowWrite();
                            });

                        ft.Field(f => f.Fixture.Goals)
                            .HasName("Goals")
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
                e.Field(p => p.Name)
                    .AllowWrite();

                e.Field(p => p.SquadNumber)
                    .HasName("Number");

                e.Field(p => p.Goals)
                    .IsCollection(g => { g.Field(h => h.Id); });

                e.Field(p => p.Team)
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