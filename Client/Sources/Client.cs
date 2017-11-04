using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Coinche.Protobuf;

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
                while (client.IsAlive())
                {
                    // Wait
                }
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
                    var bytesFrom = new byte[1024];
                    Stream.Read(bytesFrom, 0, 1024);
                    var dataFromServer = Encoding.ASCII.GetString(bytesFrom);
                    Console.Out.WriteLineAsync(dataFromServer);
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
                    var message = new Message(Console.In.ReadLine());
                    Stream.Write(message.ProtobufTypeAsBytes, 0, 2);
                    ProtoBuf.Serializer.SerializeWithLengthPrefix(Stream, message, ProtoBuf.PrefixStyle.Fixed32);
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