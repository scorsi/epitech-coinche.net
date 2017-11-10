using System.Collections.Generic;

namespace Lib.Sources.Game.Card
{
    public class Team
    {
        public enum ETeam
        {
            Red = 0,
            Blue = 1
        }

        private static readonly Dictionary<ETeam, Team> TeamMappings = new Dictionary<ETeam, Team>();
        
        public static readonly Team RedTeam = new Team(ETeam.Red, "Red");
        public static readonly Team BlueTeam = new Team(ETeam.Blue, "Blue");        

        private ETeam Index { get; }
        private string Name { get; }

        private Team(ETeam index, string name)
        {
            Index = index;
            Name = name;
            TeamMappings[index] = this;
        }

        public static Team From(ETeam team)
        {
            return TeamMappings[team];
        }

        public static Team From(int id)
        {
            return TeamMappings[(ETeam) id];
        }
    }
}