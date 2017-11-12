using System;
using Lib;

namespace Coinche.Protobuf
{
    public abstract class Wrapper
    {
        public enum Type
        {
            Unknown = 0,
            Message = 1,
            LobbyCreate = 2,
            LobbyJoin = 3,
            LobbyLeave = 4,
            LobbyList = 5,
            LobbyTeam = 6,
            LobbyCard = 7,
            LobbyContract = 8,
            LobbyShowCards = 9
        }
        
        public Type ProtobufType { get; }
        public byte[] ProtobufTypeAsBytes { get; }

        protected Wrapper() {
            var t = GetType();
            if (t == typeof(Message))
                ProtobufType = Type.Message;
            else if (t == typeof(LobbyCreate))
                ProtobufType = Type.LobbyCreate;
            else if (t == typeof(LobbyJoin))
                ProtobufType = Type.LobbyJoin;
            else if (t == typeof(LobbyLeave))
                ProtobufType = Type.LobbyLeave;
            else if (t == typeof(LobbyList))
                ProtobufType = Type.LobbyList;
            else if (t == typeof(LobbyTeam))
                ProtobufType = Type.LobbyTeam;
            else if (t == typeof(LobbyCard))
                ProtobufType = Type.LobbyCard;
            else if (t == typeof(LobbyContract))
                ProtobufType = Type.LobbyContract;
            else if (t == typeof(LobbyShowCards))
                ProtobufType = Type.LobbyShowCards;
            else
            {
                ProtobufType = Type.Unknown;
                throw new Exception("Object type unknown");
            }
            
            ProtobufTypeAsBytes = BitConverter.GetBytes((short) ProtobufType);
        }
    }
}