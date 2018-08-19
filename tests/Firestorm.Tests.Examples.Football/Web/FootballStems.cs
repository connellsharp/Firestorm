using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Stems;
using Firestorm.Tests.Examples.Football.Models;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Roots.DataSource;

namespace Firestorm.Tests.Examples.Football.Web
{
    [DataSourceRoot]
    public class PlayersStem : Stem<Player>
    {
        [Get, Set]
        public static string Name { get; }

        [Get]
        public static int SquadNumber { get; }
    }

    [DataSourceRoot]
    public class FixturesStem : Stem<FixtureTeam>
    {
        [Get, Identifier]
        public static Expression<Func<FixtureTeam, int>> Id { get; } = ft => ft.FixtureId;

        [Get]
        public static Expression<Func<FixtureTeam, bool>> Home { get; } = ft => ft.IsHome;

        [Set]
        public static void SetHome(FixtureTeam ft, bool isHome)
        {
            ft.IsHome = isHome;

            var vsTeam = ft.Fixture.Teams.FirstOrDefault(tt => tt.TeamId != ft.TeamId);
            if (vsTeam != null)
                vsTeam.IsHome = !isHome;
        }

        [Get, Substem(typeof(VsTeamStem))]
        public static Expression<Func<FixtureTeam, FixtureTeam>> VsTeam
        {
            get { return ft => ft.Fixture.Teams.FirstOrDefault(tt => tt.TeamId != ft.TeamId); }
        }

        [Set]
        public static void SetVsTeam(FixtureTeam ft, FixtureTeam vsTeam)
        {
            if (ft.Fixture == null)
                ft.Fixture = new Fixture { Teams = new List<FixtureTeam>() };

            ft.Fixture.Teams.Add(vsTeam);
        }

        [Get, Substem(typeof(GoalsStem))]
        public static Expression<Func<FixtureTeam, ICollection<Goal>>> Goals { get; } = ft => ft.Fixture.Goals;
    }

    [DataSourceRoot] // TODO not
    public class VsTeamStem : Stem<FixtureTeam>
    {
        [Get, Set]
        public static Expression<Func<FixtureTeam, int>> Id { get; } = ft => ft.TeamId;
    }

    [DataSourceRoot]
    public class GoalsStem : Stem<Goal>
    {
    }

    [DataSourceRoot]
    public class TeamsStem : Stem<Team>
    {
        [Get]
        public static string Name { get; }

        [Get]
        public static int FoundedYear { get; }

        [Get, Substem(typeof(PlayersStem))]
        public static ICollection<Player> Players { get; }

        [Get, Substem(typeof(FixturesStem))]
        public static ICollection<FixtureTeam> Fixtures { get; }

        [Get]
        public static League League { get; }
    }

    [DataSourceRoot]
    public class LeaguesStem : Stem<League>
    {
        [Get, Identifier]
        public static Expression<Func<League, string>> Key { get; } = l => l.Name.Replace(" ", string.Empty).ToLower();

        [Get]
        public static string Name { get; }

        [Get, Substem(typeof(TeamPositionsStem))]
        public static Expression<Func<League, IEnumerable<TeamPosition>>> Teams
        {
            get
            {
                return l => l.Teams
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
                    });
            }
        }
    }

    [DataSourceRoot]
    public class TeamPositionsStem : Stem<TeamPosition>
    {
        [Get]
        public static int Points { get; }

        [Get]
        public static string Team { get; }
    }
}
