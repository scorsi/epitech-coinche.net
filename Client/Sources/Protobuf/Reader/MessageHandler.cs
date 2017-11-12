using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;

namespace Coinche.Client.Protobuf.Reader
{
    public class MessageHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var message = ProtoBuf.Serializer.DeserializeWithLengthPrefix<Message>(stream, ProtoBuf.PrefixStyle.Fixed32);

            Console.Out.WriteLineAsync(message.Text);
            
            return true;
        }
    }
}