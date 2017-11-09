using System;
using System.Collections;
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
        private Hashtable ReadHandlers { get; } = new Hashtable()
        {
            { Wrapper.Type.Message, new Protobuf.Reader.MessageHandler() },
            { Wrapper.Type.LobbyList, new Protobuf.Reader.Lobby.ListHandler() }
        };
        
        /**
         * WriteManager
         */
        private WriteManager WriteManager { get; } = new WriteManager();
        private Hashtable WriteHandlers { get; } = new Hashtable()
        {
            { Wrapper.Type.Message, new Protobuf.Writer.MessageHandler() },
            { Wrapper.Type.LobbyCreate, new Protobuf.Writer.Lobby.CreateHandler() },
            { Wrapper.Type.LobbyJoin, new Protobuf.Writer.Lobby.JoinHandler() },
            { Wrapper.Type.LobbyList, new Protobuf.Writer.Lobby.ListHandler() },
            { Wrapper.Type.LobbyLeave, new Protobuf.Writer.Lobby.LeaveHandler() }
        };

        /**
         * InputManager
         */
        private InputManager InputManager { get; } = new InputManager();
        private Hashtable InputInfos { get; } = new Hashtable()
        {
            { new string[]{"/create", "/c"}, Wrapper.Type.LobbyCreate },
            { new string[]{"/join", "/j"}, Wrapper.Type.LobbyJoin },
            { new string[]{"/leave", "/l"}, Wrapper.Type.LobbyLeave },
            { new string[]{"/list-lobbies", "/ll"}, Wrapper.Type.LobbyList }
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