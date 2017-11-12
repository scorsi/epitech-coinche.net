using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;

namespace Coinche.Server.Protobuf.Reader.Lobby
{
    public class ShowCardHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyShowCards>(stream, ProtoBuf.PrefixStyle.Fixed32);
            var client = Server.Singleton.ClientList[clientId];
            if (client.Lobby.HandleAction(proto, client))
            {
                Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyShowCards, clientId.ToString());                
            }
            else
            {
                Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyShowCards);                
            }
            return true;
        }        
    }
}