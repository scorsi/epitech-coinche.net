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
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyCreate>(stream, ProtoBuf.PrefixStyle.Fixed32);
            var lobby = new Coinche.Server.Lobby(proto.Name);
            lobby.Info.Clients.Add(((Client) Server.Singleton.ClientList[clientId]).Info);
            Server.Singleton.LobbyList.Add(lobby);
            return true;
        }
    }
}