using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;

namespace Coinche.Server.Protobuf.Reader
{
    public class MessageHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId)
        {
            var message = ProtoBuf.Serializer.DeserializeWithLengthPrefix<Message>(stream, ProtoBuf.PrefixStyle.Fixed32);

            var client = (Client) Server.Singleton.ClientList[clientId];
            
            if (client.Lobby != null)
                client.Lobby.Broadcast(message.Text, true, client);
            else
                Server.Singleton.Broadcast(message.Text, true, client);

            return true;
        }
    }
}