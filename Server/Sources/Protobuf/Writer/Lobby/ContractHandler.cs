using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;
using Lib;

namespace Coinche.Server.Protobuf.Writer.Lobby
{
    public class ContractHandler : IWriter
    {
        public bool Run(NetworkStream stream, string input)
        {
            var proto = new LobbyContract {ContractType = (ContractInfo.EType) Convert.ToInt32(input)};
            stream.Write(proto.ProtobufTypeAsBytes, 0, 2);
            ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, proto, ProtoBuf.PrefixStyle.Fixed32);
            return true;
        }
    }
}