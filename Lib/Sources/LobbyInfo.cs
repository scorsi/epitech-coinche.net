using System.Collections.Generic;
using ProtoBuf;

namespace Lib.Sources
{
    [ProtoContract]
    public class LobbyInfo
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public List<ClientInfo> Clients { get; set; } = new List<ClientInfo>();

        public LobbyInfo(string name) => Name = name;
    }
}