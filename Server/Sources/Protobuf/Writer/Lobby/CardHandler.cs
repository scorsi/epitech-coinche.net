using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;
using Lib;
using Lib.Game.Card;

namespace Coinche.Server.Protobuf.Writer.Lobby
{
    public class CardHandler : IWriter
    {
        public bool Run(NetworkStream stream, string input)
        {
            var args = Regex.Split(input, @"\s+");
            var proto = new LobbyCard
            {
                Info = new CardInfo()
            };
            if (args.Length < 2)
            {
                proto.Info.Face = null;
                proto.Info.Color = null;
            }
            else
            {
                proto.Info.FaceId = (CardFace.EFace) Convert.ToInt32(args[0]);
                proto.Info.ColorId = (CardColor.EColor) Convert.ToInt32(args[1]);
            }
            stream.Write(proto.ProtobufTypeAsBytes, 0, 2);
            ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, proto, ProtoBuf.PrefixStyle.Fixed32);
            return true;
        }
    }
}