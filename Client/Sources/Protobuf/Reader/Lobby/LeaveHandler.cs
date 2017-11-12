using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;

namespace Coinche.Client.Protobuf.Reader.Lobby
{
    public class LeaveHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyLeave>(stream,
                ProtoBuf.PrefixStyle.Fixed32);
            if (proto.Name != null)
                Console.Out.WriteLineAsync("You successfully left the lobby '" + proto.Name + "'.");
            else
                Console.Out.WriteLineAsync("You're not in a lobby.");
            return true;
        }
    }
}