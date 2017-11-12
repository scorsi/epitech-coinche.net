using System.Collections.Generic;
using Lib;
using ProtoBuf;

namespace Coinche.Protobuf
{
    [ProtoContract]
    public class LobbyShowCards : Wrapper
    {
        [ProtoMember(1)]
        public List<CardInfo> Cards { get; set; }

        public LobbyShowCards()
        {
        }
    }
}