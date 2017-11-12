using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;
using Coinche.Protobuf.Writer;
using Coinche.Client.Inputs;

namespace Coinche.Client
{
    internal class Client
    {
        /**
         * Main
         */
        public static void Main(string[] args)
        {
            var client = new Client();
            if (!client.IsInitialized) return;
            if (client.Run())
            {
                do
                {
                    Thread.Sleep(50);
                } while (client.IsAlive());
            }
            client.Clear();
        }

        /**
         * Default configuration
         */
        private const string DefaultServerIp = "127.0.0.1";
        private const int DefaultServerPort = 8888;
        
        /**
         * Socket
         */
        public TcpClient Socket { get; private set; } = new TcpClient();
        private NetworkStream Stream { get; set; } = default(NetworkStream);
        
        /**
         * Threads
         */
        private Thread ReadThread { get; set; }
        private Thread WriteThread { get; set; }

        /**
         * RequestManager
         */
        private ReadManager ReadManager { get; } = new ReadManager();
        private Dictionary<Wrapper.Type, IReader> ReadHandlers { get; } = new Dictionary<Wrapper.Type, IReader>
        {
            { Wrapper.Type.Message, new Protobuf.Reader.MessageHandler() },
            { Wrapper.Type.LobbyList, new Protobuf.Reader.Lobby.ListHandler() },
            { Wrapper.Type.LobbyJoin, new Protobuf.Reader.Lobby.JoinHandler() },
            { Wrapper.Type.LobbyLeave, new Protobuf.Reader.Lobby.LeaveHandler() },
            { Wrapper.Type.LobbyCreate, new Protobuf.Reader.Lobby.CreateHandler() },
            { Wrapper.Type.LobbyTeam, new Protobuf.Reader.Lobby.TeamHandler() },
            { Wrapper.Type.LobbyCard, new Protobuf.Reader.Lobby.CardHandler() },
            { Wrapper.Type.LobbyContract, new Protobuf.Reader.Lobby.ContractHandler() },
            { Wrapper.Type.LobbyShowCards, new Protobuf.Reader.Lobby.ShowCardHandler() }
        };
        
        /**
         * WriteManager
         */
        private WriteManager WriteManager { get; } = new WriteManager();
        private Dictionary<Wrapper.Type, IWriter> WriteHandlers { get; } = new Dictionary<Wrapper.Type, IWriter>
        {
            { Wrapper.Type.Message, new Protobuf.Writer.MessageHandler() },
            { Wrapper.Type.LobbyCreate, new Protobuf.Writer.Lobby.CreateHandler() },
            { Wrapper.Type.LobbyJoin, new Protobuf.Writer.Lobby.JoinHandler() },
            { Wrapper.Type.LobbyList, new Protobuf.Writer.Lobby.ListHandler() },
            { Wrapper.Type.LobbyLeave, new Protobuf.Writer.Lobby.LeaveHandler() },
            { Wrapper.Type.LobbyTeam, new Protobuf.Writer.Lobby.TeamHandler() },
            { Wrapper.Type.LobbyCard, new Protobuf.Writer.Lobby.CardHandler() },
            { Wrapper.Type.LobbyContract, new Protobuf.Writer.Lobby.ContractHandler() },
            { Wrapper.Type.LobbyShowCards, new Protobuf.Writer.Lobby.ShowCardHandler() }
        };

        /**
         * InputManager
         */
        private InputManager InputManager { get; } = new InputManager();
        private Dictionary<string[], Wrapper.Type> InputInfos { get; } = new Dictionary<string[], Wrapper.Type>
        {
            { new string[]{"/create", "/c"}, Wrapper.Type.LobbyCreate },
            { new string[]{"/join", "/j"}, Wrapper.Type.LobbyJoin },
            { new string[]{"/leave", "/l"}, Wrapper.Type.LobbyLeave },
            { new string[]{"/list-lobbies", "/ll"}, Wrapper.Type.LobbyList },
            { new string[]{"/team", "/t"}, Wrapper.Type.LobbyTeam },
            { new string[]{"/play-card", "/card", "/pc"}, Wrapper.Type.LobbyCard },
            { new string[]{"/contract"}, Wrapper.Type.LobbyContract },
            { new string[]{"/show-cards", "/sc"}, Wrapper.Type.LobbyShowCards }
        };

        /**
         * Serve to known if the client has been properly initialized
         */
        private bool IsInitialized { get; set; }

        /**
         * Default constructor
         */
        private Client()
        {
            Initialize(DefaultServerIp, DefaultServerPort);
        }

        /**
         * Constructor with custom parameters
         */
        private Client(string ip, int port)
        {
            Initialize(ip, port);
        }

        /**
         * Initialize method which will create the socket
         */
        private void Initialize(string ip, int port)
        {
            IsInitialized = false;
            try
            {
                ReadManager.Initialize(ReadHandlers);
                WriteManager.Initialize(WriteHandlers);
                InputManager.Initialize(InputInfos);
                Socket.Connect(ip, port);
                Stream = Socket.GetStream();
                IsInitialized = true;
            }
            catch (Exception e)
            {
                Console.Out.WriteLineAsync(e.ToString());
            }
        }

        /**
         * Run two different threads, one for read and the second for write
         */
        private bool Run()
        {
            try
            {
                ReadThread = new Thread(ThreadedReadRun);
                WriteThread = new Thread(ThreadedWriteRun);

                ReadThread.Start();
                WriteThread.Start();
                
                return true;
            }
            catch (Exception e)
            {
                Console.Out.WriteLineAsync(e.ToString());
                return false;
            }
        }

        /**
         * Read from the server and write to the console
         */
        private void ThreadedReadRun()
        {
            while (IsAlive())
            {
                try
                {
                    ReadManager.Run(Stream);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        /**
         * Read from the console and write to the server
         */
        private void ThreadedWriteRun()
        {
            while (IsAlive())
            {
                try
                {
                    InputManager.Run(Stream, WriteManager, Console.In.ReadLine());
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
        
        /**
         * Serve to know of threads and socket are still alive
         */
        private bool IsAlive()
        {
            return ReadThread.IsAlive && WriteThread.IsAlive && Socket.Connected;
        }

        /**
         * Clear socket and threads
         */
        private void Clear()
        {
            if (ReadThread.IsAlive)
                ReadThread.Abort();
            
            if (WriteThread.IsAlive)
                WriteThread.Abort();
            
            Socket.Close();
        }
        
    }
}