using System.Collections.Generic;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;
using Lib;

namespace Coinche.Server.Protobuf.Writer.Lobby
{
    public class ListHandler : IWriter
    {
        public bool Run(NetworkStream stream, string input)
        {
            var proto = new LobbyList { LobbyInfos = new List<LobbyInfo>() };
            foreach (var lobby in Server.Singleton.LobbyList)
            {
                proto.LobbyInfos.Add(lobby.Info);
            }
            stream.Write(proto.ProtobufTypeAsBytes, 0, 2);
            ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, proto, ProtoBuf.PrefixStyle.Fixed32);
            return true;
        }
    }
}