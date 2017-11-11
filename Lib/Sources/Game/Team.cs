using System.Collections.Generic;
using System.Linq;

namespace Lib.Game.Card
{
    public class Team
    {
        public enum ETeam
        {
            Undefined = 1,
            Red = 2,
            Blue = 3
        }

        private static readonly Dictionary<ETeam, Team> TeamMappings = new Dictionary<ETeam, Team>();
        
        public static readonly Team Undefined = new Team(ETeam.Undefined, "undefined");
        public static readonly Team Red = new Team(ETeam.Red, "red");
        public static readonly Team Blue = new Team(ETeam.Blue, "blue");        

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

        public static Team From(string name)
        {
            return (from Entry in TeamMappings where Entry.Value.Name.Equals(name) select Entry.Value).FirstOrDefault();
        }
    }
}