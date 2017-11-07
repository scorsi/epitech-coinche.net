using ProtoBuf;

namespace Coinche.Protobuf
{
    [ProtoContract]
    public class LobbyJoin : Wrapper
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        public LobbyJoin() => Name = "";
        public LobbyJoin(string name) => Name = name;
    }
}