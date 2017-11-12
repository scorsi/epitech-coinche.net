using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;
using Lib;
using Lib.Game.Card;

namespace Coinche.Client.Protobuf.Reader.Lobby
{
    public class CardHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyCard>(stream,
                ProtoBuf.PrefixStyle.Fixed32);
            if (proto.Info?.Face != null && proto.Info?.Color != null)
                Console.Out.WriteLineAsync("You successfully played the " + proto.Info.Face.Name + " of " + proto.Info.Color.Name + ".");
            else
                Console.Out.WriteLineAsync("You didn't play the card because it's not you turn or you don't have it.");
            return true;
        }        
    }
}