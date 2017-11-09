using ProtoBuf;

namespace Coinche.Protobuf
{
    [ProtoContract]
    public class LobbyLeave : Wrapper
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        public LobbyLeave() => Name = null;
        public LobbyLeave(string name) => Name = name;
    }
}