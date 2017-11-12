using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;

namespace Coinche.Client.Protobuf.Writer.Lobby
{
    public class ShowCardHandler : IWriter
    {
        public bool Run(NetworkStream stream, string input)
        {
            var proto = new LobbyShowCards();
            stream.Write(proto.ProtobufTypeAsBytes, 0, 2);
            ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, proto, ProtoBuf.PrefixStyle.Fixed32);
            return true;
        }
    }
}