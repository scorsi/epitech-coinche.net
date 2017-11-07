using System.Net.Sockets;
using System.Text.RegularExpressions;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;

namespace Coinche.Client.Protobuf.Writer.Lobby
{
    public class JoinHandler : IWriter
    {
        public bool Run(NetworkStream stream, string input)
        {
            var args = Regex.Split(input, @"\s+");
            if (args.Length < 2 || args[1].Length <= 0)
                return false;

            var proto = new LobbyJoin(args[1]);
            stream.Write(proto.ProtobufTypeAsBytes, 0, 2);
            ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, proto, ProtoBuf.PrefixStyle.Fixed32);
            return true;
        }
    }
}