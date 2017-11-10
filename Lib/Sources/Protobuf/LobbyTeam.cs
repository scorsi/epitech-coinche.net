using ProtoBuf;

namespace Coinche.Protobuf
{
    [ProtoContract]
    public class LobbyTeam
    {
        [ProtoMember(1)]
        public int Team { get; set; }

        public LobbyTeam() => Team = (int) Lib.Game.Card.Team.ETeam.Undefined;
        public LobbyTeam(int value) => Team = value;
    }
}