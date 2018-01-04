using System;
using System.Collections.Generic;
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
    public class TeamsStem : Stem<Player>
    {
        [Get]
        public static string Name { get; }

        [Get]
        public static int SquadNumber { get; }
    }

    [DataSourceRoot]
    public class LeaguesStem : Stem<League>
    {
        [Get, Identifier]
        public static Expression<Func<League, string>> Key { get; } = l => l.Name.Replace(" ", string.Empty).ToLower();

        [Get]
        public static string Name { get; }

        [Get]
        public static int SquadNumber { get; }
    }
}
