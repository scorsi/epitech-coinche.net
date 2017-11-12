using ProtoBuf;

namespace Coinche.Protobuf
{
    [ProtoContract]
    public class LobbyTeam : Wrapper
    {
        [ProtoMember(1)]
        public int Team { get; set; }

        public LobbyTeam()
        {
        }
        
        public LobbyTeam(int value) => Team = value;
    }
}