using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;

namespace Coinche.Client.Protobuf.Reader.Lobby
{
    public class JoinHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyJoin>(stream,
                ProtoBuf.PrefixStyle.Fixed32);
            if (proto.Name != null)
                Console.Out.WriteLineAsync("You successfully joined the lobby '" + proto.Name + "'.");
            else
                Console.Out.WriteLineAsync("You didn't join the lobby because you're already in one or it doesn't exist.");
            return true;
        }        
    }
}