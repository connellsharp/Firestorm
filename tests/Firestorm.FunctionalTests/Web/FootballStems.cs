using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.FunctionalTests.Models;
using Firestorm.Stems;
using Firestorm.Stems.Essentials;
using Firestorm.Stems.Roots.DataSource;

namespace Firestorm.FunctionalTests.Web
{
    public class PlayersStem : Stem<Player>
    {
        [Get, Set, AutoExpr]
        public static string Name { get; }

        [Get,  AutoExpr]
        public static int SquadNumber { get; }
    }

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

    [NoDataSourceRoot]
    public class VsTeamStem : Stem<FixtureTeam>
    {
        [Get, Set]
        public static Expression<Func<FixtureTeam, int>> Id { get; } = ft => ft.TeamId;
    }

    public class GoalsStem : Stem<Goal>
    {
    }

    public class TeamsStem : Stem<Team>
    {
        [Get, AutoExpr]
        public static string Name { get; }

        [Get, AutoExpr]
        public static int FoundedYear { get; }

        [Get, Substem(typeof(PlayersStem)), AutoExpr]
        public static ICollection<Player> Players { get; }

        [Get, Substem(typeof(FixturesStem)), AutoExpr]
        public static ICollection<FixtureTeam> Fixtures { get; }

        [Get, Substem(typeof(LeaguesStem)), AutoExpr]
        public static League League { get; }
    }

    public class LeaguesStem : Stem<League>
    {
        [Get, Identifier]
        public static Expression<Func<League, string>> Key { get; } = l => l.Name.Replace(" ", string.Empty).ToLower();

        [Get, AutoExpr]
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
                        Wins = t.Fixtures.Count(f => f.Fixture.Goals.Count(g => g.Player.Team == t) > f.Fixture.Goals.Count(g => g.Player.Team != t)),
                        Draws = t.Fixtures.Count(f => f.Fixture.Goals.Count(g => g.Player.Team == t) == f.Fixture.Goals.Count(g => g.Player.Team != t)),
                        Losses = t.Fixtures.Count(f => f.Fixture.Goals.Count(g => g.Player.Team == t) < f.Fixture.Goals.Count(g => g.Player.Team != t)),
                    })
                    .Select(t => new TeamPosition
                    {
                        Points = (t.Wins * 3) + (t.Draws * 1),
                        Team = t.Team.Name
                    });
            }
        }
    }

    [NoDataSourceRoot]
    public class TeamPositionsStem : Stem<TeamPosition>
    {
        [Get, AutoExpr]
        public static int Points { get; }

        [Get, AutoExpr]
        public static string Team { get; }
    }
}
