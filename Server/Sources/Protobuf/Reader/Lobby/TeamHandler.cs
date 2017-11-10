using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;

namespace Coinche.Server.Protobuf.Reader.Lobby
{
    public class TeamHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyTeam>(stream, ProtoBuf.PrefixStyle.Fixed32);
            var client = Server.Singleton.ClientList[clientId];
            client.Lobby.HandleAction(proto, client);
            Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyTeam, client.Info.TeamId.ToString());
            return true;
        }
    }
}