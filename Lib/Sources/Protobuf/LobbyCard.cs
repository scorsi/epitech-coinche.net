using Lib;
using ProtoBuf;

namespace Coinche.Protobuf
{
    [ProtoContract]
    public class LobbyCard : Wrapper
    {
        [ProtoMember(1)]
        public CardInfo Info { get; set; }

        public LobbyCard()
        {
        }
    }
}