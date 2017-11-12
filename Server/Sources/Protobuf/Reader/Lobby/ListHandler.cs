using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;

namespace Coinche.Server.Protobuf.Reader.Lobby
{
    public class ListHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyList>(stream, ProtoBuf.PrefixStyle.Fixed32);
            Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyList);
            return true;
        }
    }
}