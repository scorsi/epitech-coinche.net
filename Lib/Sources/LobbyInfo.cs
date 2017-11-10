using System.Collections.Generic;
using Lib.Game.Card;
using ProtoBuf;

namespace Lib
{
    [ProtoContract]
    public class LobbyInfo
    {
        [ProtoMember(1)]
        public string Name { get; set; } = "";
        
        [ProtoMember(2)]
        public List<ClientInfo> Clients { get; set; } = new List<ClientInfo>(); 
        
        [ProtoMember(3)]
        public Dictionary<Team, int> Points { get; set; } = new Dictionary<Team, int>();

        public LobbyInfo()
        {
        }
        
        public LobbyInfo(string name) => Name = name;

        public void ResetPoints()
        {
            Points[Team.Blue] = 0;
            Points[Team.Red] = 0;
        }
    }
}