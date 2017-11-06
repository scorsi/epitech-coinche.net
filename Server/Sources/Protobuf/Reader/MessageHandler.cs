using System;
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

            Console.Out.WriteLineAsync("Received message from client " + clientId + " : " + message.Text);
            
            Server.Singleton.Broadcast(message.Text);
            Server.Singleton.Broadcast(message.Text, true, (Client) Server.Singleton.ClientList[clientId]);
            
            return true;
        }
    }
}