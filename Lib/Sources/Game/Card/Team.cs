using System.Collections.Generic;

namespace Lib.Game.Card
{
    public class Team
    {
        public enum ETeam
        {
            Undefined = -1,
            Red = 0,
            Blue = 1
        }

        private static readonly Dictionary<ETeam, Team> TeamMappings = new Dictionary<ETeam, Team>();
        
        public static readonly Team Undefined = new Team(ETeam.Undefined, "Undefined");
        public static readonly Team Red = new Team(ETeam.Red, "Red");
        public static readonly Team Blue = new Team(ETeam.Blue, "Blue");        

        public ETeam Index { get; }
        public string Name { get; }

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