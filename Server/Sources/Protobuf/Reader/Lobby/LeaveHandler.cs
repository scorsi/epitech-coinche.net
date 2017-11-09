using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;

namespace Coinche.Server.Protobuf.Reader.Lobby
{
    public class LeaveHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyLeave>(stream, ProtoBuf.PrefixStyle.Fixed32);
            try
            {
                foreach (var lobby in Server.Singleton.LobbyList)
                {
                    if (lobby.Info.Clients.Contains(((Client) Server.Singleton.ClientList[clientId]).Info))
                    {
                        lobby.Info.Clients.Remove(((Client)Server.Singleton.ClientList[clientId]).Info);
                        Server.Singleton.WriteManager.Run(stream, Wrapper.Type.LobbyLeave, lobby.Info.Name);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }
    }
}