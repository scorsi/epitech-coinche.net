using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;

namespace Coinche.Client.Protobuf.Writer
{
    public class MessageHandler : IWriter
    {
        public bool Run(NetworkStream stream, string input)
        {
            var message = new Message(input);
            stream.Write(message.ProtobufTypeAsBytes, 0, 2);
            ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, message, ProtoBuf.PrefixStyle.Fixed32);
            return true;
        }
    }
}