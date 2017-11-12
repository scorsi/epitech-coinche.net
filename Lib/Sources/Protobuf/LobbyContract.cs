using Lib;
using ProtoBuf;

namespace Coinche.Protobuf
{
    [ProtoContract]
    public class LobbyContract : Wrapper
    {
        [ProtoMember(1)]
        public ContractInfo.EType ContractType { get; set; }
        
        [ProtoMember(2)]
        public int ContractValue { get; set; }

        public LobbyContract()
        {
        }

        public LobbyContract(ContractInfo.EType type, int value)
        {
            ContractType = type;
            ContractValue = value;
        }
    }
}