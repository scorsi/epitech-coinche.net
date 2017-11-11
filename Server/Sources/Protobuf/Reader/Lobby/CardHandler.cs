using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;
using Lib.Game.Card;

namespace Coinche.Server.Protobuf.Reader.Lobby
{
    public class CardHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyCard>(stream, ProtoBuf.PrefixStyle.Fixed32);
            try
            {
                Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyCard, (int) proto.Info.FaceId + " " + (int) proto.Info.ColorId);
                return true;
            }
            catch (Exception)
            {
                Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyCard, CardFace.EFace.Undefined + " " + CardColor.EColor.Undefined);
                return false;
            }
        }        
    }
}