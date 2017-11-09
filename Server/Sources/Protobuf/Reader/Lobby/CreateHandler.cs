using System.Linq;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;
using Lib.Sources;

namespace Coinche.Server.Protobuf.Reader.Lobby
{
    public class CreateHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            if (Server.Singleton.LobbyList.Any(tmpLobby => tmpLobby.Info.Clients.Contains(((Client) Server.Singleton.ClientList[clientId]).Info)))
            {
                Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyCreate);
                return false;
            }

            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyCreate>(stream, ProtoBuf.PrefixStyle.Fixed32);            
            var lobby = new Coinche.Server.Lobby(proto.Name);
            lobby.AddClient((Client) Server.Singleton.ClientList[clientId]);
            Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyCreate, proto.Name);                
            Server.Singleton.LobbyList.Add(lobby);
            return true;
        }
    }
}