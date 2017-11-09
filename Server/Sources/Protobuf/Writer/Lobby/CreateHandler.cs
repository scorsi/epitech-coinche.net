using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;

namespace Coinche.Server.Protobuf.Writer.Lobby
{
    public class CreateHandler : IWriter
    {
        public bool Run(NetworkStream stream, string input)
        {
            var proto = new LobbyCreate();
            proto.Name = input;
            stream.Write(proto.ProtobufTypeAsBytes, 0, 2);
            ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, proto, ProtoBuf.PrefixStyle.Fixed32);
            return true;
        }        
    }
}