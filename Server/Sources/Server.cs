using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;
using Coinche.Protobuf.Writer;

namespace Coinche.Server
{
    internal class Server
    {
        public static void Main(string[] args)
        {
            if (!Singleton.Initialize()) return;
            Singleton.Run();
            Singleton.Clear();
        }

        private const int DefaultServerPort = 8888;

        /**
         * Singleton
         */
        public static Server Singleton { get; } = new Server();

        /**
         * Socket Listener
         */
        private TcpListener Listener { get; set; }

        /**
         * ReadManager
         */
        private ReadManager ReadManager { get; } = new ReadManager();
        private Dictionary<Wrapper.Type, IReader> ReadHandlers { get; } = new Dictionary<Wrapper.Type, IReader>
        {
            { Wrapper.Type.Message, new Protobuf.Reader.MessageHandler() },
            { Wrapper.Type.LobbyCreate, new Protobuf.Reader.Lobby.CreateHandler() },
            { Wrapper.Type.LobbyJoin, new Protobuf.Reader.Lobby.JoinHandler() },
            { Wrapper.Type.LobbyLeave, new Protobuf.Reader.Lobby.LeaveHandler() },
            { Wrapper.Type.LobbyList, new Protobuf.Reader.Lobby.ListHandler() },
            { Wrapper.Type.LobbyTeam, new Protobuf.Reader.Lobby.TeamHandler() },
            { Wrapper.Type.LobbyCard, new Protobuf.Reader.Lobby.CardHandler() },
            { Wrapper.Type.LobbyContract, new Protobuf.Reader.Lobby.ContractHandler() }
        };

        /**
         * WriteManager
         */
        public WriteManager WriteManager { get; } = new WriteManager();
        private Dictionary<Wrapper.Type, IWriter> WriteHandlers { get; } = new Dictionary<Wrapper.Type, IWriter>
        {
            { Wrapper.Type.Message, new Protobuf.Writer.MessageHandler() },
            { Wrapper.Type.LobbyList, new Protobuf.Writer.Lobby.ListHandler() },
            { Wrapper.Type.LobbyJoin, new Protobuf.Writer.Lobby.JoinHandler() },
            { Wrapper.Type.LobbyLeave, new Protobuf.Writer.Lobby.LeaveHandler() },
            { Wrapper.Type.LobbyCreate, new Protobuf.Writer.Lobby.CreateHandler() },
            { Wrapper.Type.LobbyTeam, new Protobuf.Writer.Lobby.TeamHandler() },
            { Wrapper.Type.LobbyCard, new Protobuf.Writer.Lobby.CardHandler() },
            { Wrapper.Type.LobbyContract, new Protobuf.Writer.Lobby.ContractHandler() }
        };

        /**
         * List of connected clients
         */
        public Dictionary<int, Client> ClientList { get; private set; }
        
        /**
         * List of lobbies
         */
        public List<Lobby> LobbyList { get; private set; }

        /**
         * Temporary list for clients to be removed
         */
        private List<Client> PendingDisconnection { get; } = new List<Client>();

        /**
         * Status attributes
         */
        private bool IsInitialized { get; set; }

        /**
         * Initiliaze listener
         */
        private bool Initialize(int port = DefaultServerPort)
        {
            IsInitialized = false;
            try
            {
                ReadManager.Initialize(ReadHandlers);
                WriteManager.Initialize(WriteHandlers);
                
                Listener = new TcpListener(IPAddress.Any, port);
                
                ClientList = new Dictionary<int, Client>();
                LobbyList = new List<Lobby>();

                Listener.Start();
                IsInitialized = true;
                return true;
            }
            catch (Exception e)
            {
                Console.Out.WriteLineAsync(e.ToString());
                return false;
            }
        }

        /**
         * Run the server
         */
        private void Run()
        {
            if (!IsInitialized)
                return;

            var nb = 0;
            while (true)
            {
                var clientSocket = Listener.AcceptTcpClient();
                ++nb;

                Console.Out.WriteLineAsync("Client " + nb + " joined the server.");

                var client = new Client(clientSocket, nb);
                client.Initialize();
                ClientList.Add(nb, client);
                
                // Check disconnection after all new clients
                CheckDisconnection();
            }
        }

        /**
         * Handle pending client disconnection
         */
        private void CheckDisconnection()
        {
            foreach (var client in PendingDisconnection)
            {
                client.Clear();
                Console.Out.WriteLineAsync("Client " + client.Info.Id + " has disconnected.");
                ClientList.Remove(client.Info.Id);
            }
            PendingDisconnection.Clear();
        }

        /**
         * Clear everything
         */
        private void Clear()
        {
            PendingDisconnection.Clear();
            foreach (var item in ClientList)
            {
                var client = item.Value;
                client.Clear();
                Console.Out.WriteLineAsync("Client " + client.Info.Id + " has disconnected.");
            }
            ClientList.Clear();
            Listener.Stop();
            IsInitialized = false;
        }

        /**
         * HandleRequest
         */
        public void HandleRequest(NetworkStream networkStream, int clientId)
        {
            ReadManager.Run(networkStream, clientId);
        }

        /**
         * Broadcast to all connected clients
         */
        public void Broadcast(string msg, bool flag = false, Client client = null)
        {
            if (flag && client == null)
                return; // Add some security for NullReferenceException
            
            foreach (var item in ClientList)
            {
                var broadcastClient = item.Value;
                if (broadcastClient == client)
                    continue; // Stop if it is the same socket
                
                if (broadcastClient.Lobby != null)
                    continue; // Stop if the client is in a Lobby

                NetworkStream broadcastStream;
                try
                {
                    broadcastStream = broadcastClient.Socket.GetStream();
                }
                catch (InvalidOperationException)
                {
                    // Remove client if we can't get its stream
                    PendingDisconnection.Add(broadcastClient);
                    continue;
                }

                WriteManager.Run(broadcastStream, Wrapper.Type.Message, 
                    flag
                        ? client.Info.Name + " says : " + msg
                        : msg);
            }
            
            // Check disconnection after all broadcast
            CheckDisconnection();
        }

        /**
         * Remove a client
         */
        public void RemoveClient(Client client)
        {
            PendingDisconnection.Add(client);
        }

    }
}