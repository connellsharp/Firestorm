using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

        public override bool CanAddItem()
        {
            return true;
        }
    }

    [DataSourceRoot]
    public class FixturesStem : Stem<FixtureTeam>
    {
        [Get, Identifier]
        public static Expression<Func<FixtureTeam, int>> ID { get; } = ft => ft.FixtureId;

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

        public override bool CanAddItem()
        {
            return true;
        }
    }

    [DataSourceRoot] // TODO not
    public class VsTeamStem : Stem<FixtureTeam>
    {
        [Get, Set]
        public static Expression<Func<FixtureTeam, int>> ID { get; } = ft => ft.TeamId;
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
    }

    [DataSourceRoot]
    public class LeaguesStem : Stem<League>
    {
        [Get, Identifier]
        public static Expression<Func<League, string>> Key { get; } = l => l.Name.Replace(" ", string.Empty).ToLower();

        [Get]
        public static string Name { get; }
    }
}
