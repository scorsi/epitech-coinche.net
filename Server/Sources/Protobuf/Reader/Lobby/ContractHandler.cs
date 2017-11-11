using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;
using Lib;

namespace Coinche.Server.Protobuf.Reader.Lobby
{
    public class ContractHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyContract>(stream, ProtoBuf.PrefixStyle.Fixed32);
            var client = Server.Singleton.ClientList[clientId];
            if (client.Lobby != null && client.Lobby.HandleAction(proto, client))
            {
                Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyContract,
                    ((int) proto.ContractType).ToString());                
            }
            else
            {
                Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyContract,
                    ((int) ContractInfo.EType.Undefined).ToString());
            }
            return true;
        }
    }
}