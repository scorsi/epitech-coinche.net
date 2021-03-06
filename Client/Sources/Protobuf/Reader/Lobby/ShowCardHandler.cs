﻿using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;

namespace Coinche.Client.Protobuf.Reader.Lobby
{
    public class ShowCardHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyShowCards>(stream,
                ProtoBuf.PrefixStyle.Fixed32);
            if (proto.Cards == null)
            {
                Console.Out.WriteLineAsync("You don't have a hand yet.");                
            }
            else if (proto.Cards.Count <= 0)
            {
                Console.Out.WriteLineAsync("You have no card left.");
            }
            else
            {
                Console.Out.WriteLineAsync("Cards in your hand:");
                foreach (var card in proto.Cards)
                    Console.Out.WriteLineAsync(card.Face.Name + " of " + card.Color.Name);
            }
            return true;
        }        
    }
}