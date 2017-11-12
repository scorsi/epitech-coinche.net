using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;
using Lib.Game.Card;

namespace Coinche.Server.Protobuf.Reader.Lobby
{
    public class TeamHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyTeam>(stream, ProtoBuf.PrefixStyle.Fixed32);
            var client = Server.Singleton.ClientList[clientId];
            if (client.Lobby != null && client.Lobby.HandleAction(proto, client))
            {
                Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyTeam, client.Info.TeamId.ToString());                
            }
            else
            {
                Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyTeam, ((int) Team.ETeam.Undefined).ToString());
            }
            return true;
        }
    }
}