using System;
using System.Collections.Generic;
using System.Text;
using Firestorm.Stems;
using Firestorm.Tests.Examples.Football.Models;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Roots.DataSource;

namespace Firestorm.Tests.Examples.Football.Web.Stems
{
    [DataSourceRoot]
    public class PlayersStem : Stem<Player>
    {
        [Get]
        public static string Name { get; }

        [Get]
        public static int SquadNumber { get; }
    }

    [DataSourceRoot]
    public class TeamsStem : Stem<Player>
    {
        [Get]
        public static string Name { get; }

        [Get]
        public static int SquadNumber { get; }
    }
}
