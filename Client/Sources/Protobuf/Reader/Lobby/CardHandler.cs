using System;
using System.Net.Sockets;
using Coinche.Protobuf.Reader;
using Lib;
using Lib.Game.Card;

namespace Coinche.Client.Protobuf.Reader.Lobby
{
    public class CardHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<CardInfo>(stream,
                ProtoBuf.PrefixStyle.Fixed32);
            if (proto.Face != -1 && proto.Color != -1)
                Console.Out.WriteLineAsync("You successfully played the " + CardFace.From(proto.Face).Name + " of " + CardColor.From(proto.Color).Name + ".");
            else
                Console.Out.WriteLineAsync("You didn't play the card because it's not you turn or you don't have it.");
            return true;
        }        
    }
}