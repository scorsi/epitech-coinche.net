using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;

namespace Coinche.Client.Protobuf.Reader.Lobby
{
    public class ListHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyList>(stream, ProtoBuf.PrefixStyle.Fixed32);
            if (proto.LobbyInfos.Count <= 0)
            {
                Console.Out.WriteLineAsync("There's no available lobby.");
                return true;
            }
            Console.Out.WriteLineAsync("List of lobbies:");
            foreach (var lobbyInfo in proto.LobbyInfos)
            {
                Console.Out.WriteLineAsync(" -> " + lobbyInfo.Name);
            }
            return true;
        }
    }
}