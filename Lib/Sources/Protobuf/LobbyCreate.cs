using ProtoBuf;

namespace Coinche.Protobuf
{
    [ProtoContract]
    public class LobbyCreate : Wrapper
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        
        public LobbyCreate() => Name = null;
        public LobbyCreate(string name) => Name = name;
    }
}