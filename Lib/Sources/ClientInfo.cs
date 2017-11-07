using ProtoBuf;

namespace Lib.Sources
{
    [ProtoContract]
    public class ClientInfo
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }

        public ClientInfo(int id)
        {
            Id = id;
            Name = Id.ToString();
        }
    }
}