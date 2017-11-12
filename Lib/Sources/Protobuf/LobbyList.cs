using System.Collections.Generic;
using Lib;
using ProtoBuf;

namespace Coinche.Protobuf
{
    [ProtoContract]
    public class LobbyList : Wrapper
    {
        [ProtoMember(1)]
        public List<LobbyInfo> LobbyInfos { get; set; }
    }
}