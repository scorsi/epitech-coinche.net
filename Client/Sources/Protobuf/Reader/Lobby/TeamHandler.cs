using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;
using Lib.Game.Card;

namespace Coinche.Client.Protobuf.Reader.Lobby
{
    public class TeamHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyTeam>(stream,
                ProtoBuf.PrefixStyle.Fixed32);
            if (Team.From(proto.Team).Name != null)
                Console.Out.WriteLineAsync("You successfully joined the " + Team.From(proto.Team).Name + " team.");
            else
                Console.Out.WriteLineAsync("You didn't join the team because it's full or it doesn't exist.");
            return true;
        }        
    }
}