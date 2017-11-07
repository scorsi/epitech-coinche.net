using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;

namespace Coinche.Server.Protobuf.Reader.Lobby
{
    public class CreateHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyCreate>(stream, ProtoBuf.PrefixStyle.Fixed32);
            Server.Singleton.LobbyList.Add(new Coinche.Server.Lobby(proto.Name));
            return true;
        }
    }
}