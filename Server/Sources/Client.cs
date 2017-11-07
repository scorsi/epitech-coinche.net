using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Coinche.Protobuf;
using Lib.Sources;

namespace Coinche.Server
{
    public class Client
    {
        public ClientInfo Info { get; set; }
        
        /**
         * Socket
         */
        public TcpClient Socket { get; }

        /**
         * Stream
         */
        private NetworkStream Stream { get; }
        
        /**
         * Thread
         */
        private Thread Thread { get; set; }

        /**
         * Constructor
         */
        public Client(TcpClient socket, int id)
        {
            Socket = socket;
            Stream = Socket.GetStream();
            Info = new ClientInfo(id);
        }

        /**
         * Initialize and start thread
         */
        public void Initialize()
        {
            Thread = new Thread(Run);
            Thread.Start();
        }

        /**
         * Run
         */
        private void Run()
        {
            while (true)
            {
                try
                {
                    Server.Singleton.HandleRequest(Stream, Info.Id);
                }
                catch (IOException)
                {
                    Server.Singleton.RemoveClient(this);
                    break;
                }
                catch (Exception e)
                {
                    Console.Out.WriteLineAsync(e.ToString());
                }
            }
        }
        
        /**
         * Clear socket and thread
         */
        public void Clear()
        {
            if (Thread.IsAlive)
                Thread.Abort();
            Socket.Close();
        }
    }
}