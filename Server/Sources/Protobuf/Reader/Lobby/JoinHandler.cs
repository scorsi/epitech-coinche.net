using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;

namespace Coinche.Server.Protobuf.Reader.Lobby
{
    public class JoinHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyJoin>(stream, ProtoBuf.PrefixStyle.Fixed32);
            try
            {
                foreach (var lobby in Server.Singleton.LobbyList)
                {
                    if (lobby.Info.Clients.Contains(((Client) Server.Singleton.ClientList[clientId]).Info))
                    {
                        Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyJoin);
                        return false;
                    }
                }
                foreach (var lobby in Server.Singleton.LobbyList)
                {
                    if (lobby.Info.Name == proto.Name)
                    {
                        lobby.Info.Clients.Add(((Client)Server.Singleton.ClientList[clientId]).Info);
                        Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyJoin, proto.Name);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyJoin);
            return false;
        }
    }
}