using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Coinche.Protobuf.Writer;
using Lib;

namespace Coinche.Server.Protobuf.Writer.Lobby
{
    public class CardHandler : IWriter
    {
        public bool Run(NetworkStream stream, string input)
        {
            var args = Regex.Split(input, @"\s+");
            var proto = new CardInfo();
            if (args.Length < 2)
            {
                proto.Face = -1;
                proto.Color = -1;
            }
            else
            {
                proto.Face = Convert.ToInt32(args[0]);
                proto.Color = Convert.ToInt32(args[1]);
            }
            stream.Write(proto.ProtobufTypeAsBytes, 0, 2);
            ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, proto, ProtoBuf.PrefixStyle.Fixed32);                
            return true;
        }
    }
}