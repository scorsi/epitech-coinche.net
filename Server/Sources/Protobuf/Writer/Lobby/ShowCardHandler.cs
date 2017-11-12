using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;
using Lib;

namespace Coinche.Server.Protobuf.Writer.Lobby
{
    public class ShowCardHandler : IWriter
    {
        public bool Run(NetworkStream stream, string input)
        {
            var proto = new LobbyShowCards() { Cards = new List<CardInfo>() };
            if (input != null)
                foreach (var card in Server.Singleton.ClientList[Convert.ToInt32(input)].Info.Deck)
                    proto.Cards.Add(card);
            else
                proto.Cards = null;
            stream.Write(proto.ProtobufTypeAsBytes, 0, 2);
            ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, proto, ProtoBuf.PrefixStyle.Fixed32);
            return true;
        }
    }
}