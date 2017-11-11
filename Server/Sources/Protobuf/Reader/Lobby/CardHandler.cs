using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;
using Lib;

namespace Coinche.Server.Protobuf.Reader.Lobby
{
    public class CardHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<CardInfo>(stream, ProtoBuf.PrefixStyle.Fixed32);
            try
            {
                Server.Singleton.WriteManager.Run(stream, Wrapper.Type.CardInfo, proto.Face + " " + proto.Color);
                return true;
            }
            catch (Exception e)
            {
                Server.Singleton.WriteManager.Run(stream, Wrapper.Type.CardInfo, "-1 -1");
                return false;
            }
        }        
    }
}