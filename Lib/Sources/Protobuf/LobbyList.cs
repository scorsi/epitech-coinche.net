using System.Collections.Generic;
using Lib.Sources;
using ProtoBuf;

namespace Coinche.Protobuf
{
    [ProtoContract]
    public class LobbyList : Wrapper
    {
        [ProtoMember(1)]
        public List<LobbyInfo> LobbyInfos { get; set; } = new List<LobbyInfo>();
    }
}