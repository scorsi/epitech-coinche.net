using System.Net.Sockets;
using System.Text.RegularExpressions;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;
using Lib.Game.Card;

namespace Coinche.Client.Protobuf.Writer.Lobby
{
    public class TeamHandler : IWriter
    {
        public bool Run(NetworkStream stream, string input)
        {
            var args = Regex.Split(input, @"\s+");
            if (args.Length < 2 || args[1].Length <= 0)
                return false;

            var proto = new LobbyTeam((int) Team.From(args[1]).Index);
            stream.Write(proto.ProtobufTypeAsBytes, 0, 2);
            ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, proto, ProtoBuf.PrefixStyle.Fixed32);
            return true;
        }        
    }
}