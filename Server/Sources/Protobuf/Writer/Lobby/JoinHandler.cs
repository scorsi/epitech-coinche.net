using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;

namespace Coinche.Server.Protobuf.Writer.Lobby
{
    public class JoinHandler : IWriter
    {
        public bool Run(NetworkStream stream, string input)
        {
            var proto = new LobbyJoin();
            if (input != null)
                proto.Name = "You succesfully joined the channel " + input + ".";
            else
                proto.Name = "Error, you're already in a channel or this channel doesn't exist.";
            stream.Write(proto.ProtobufTypeAsBytes, 0, 2);
            ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, proto, ProtoBuf.PrefixStyle.Fixed32);
            return true;
        }        
    }
}