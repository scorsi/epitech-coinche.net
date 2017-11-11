using Lib;
using ProtoBuf;

namespace Coinche.Protobuf
{
    [ProtoContract]
    public class LobbyContract : Wrapper
    {
        [ProtoMember(1)]
        public ContractInfo.EType ContractType { get; set; }

        public LobbyContract()
        {
        }
    }
}