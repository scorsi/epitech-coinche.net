﻿using System.Net.Sockets;
using System.Text.RegularExpressions;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;
using Lib;
using Lib.Game.Card;

namespace Coinche.Client.Protobuf.Writer.Lobby
{
    public class CardHandler : IWriter
    {
        public bool Run(NetworkStream stream, string input)
        {
            var args = Regex.Split(input, @"\s+");
            if (args.Length < 3 || args[1].Length <= 0 || args[2].Length <= 0)
                return false;

            var proto = new LobbyCard
            {
                Info = new CardInfo
                {
                    Face = CardFace.From(args[1].ToLower()),
                    Color = CardColor.From(args[2].ToLower())
                }
            };
            stream.Write(proto.ProtobufTypeAsBytes, 0, 2);
            ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, proto, ProtoBuf.PrefixStyle.Fixed32);
            return true;
        }
    }
}