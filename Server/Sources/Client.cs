using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Coinche.Protobuf;

namespace Coinche.Server
{
    public class Client
    {
        /**
         * Socket
         */
        public TcpClient Socket { get; }

        private NetworkStream Stream { get; }
        
        /**
         * Attribute
         */
        public int Id { get; }

        public string Username { get; }

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
            Id = id;
            Username = Id.ToString();
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
                    Server.Singleton.HandleRequest(Stream, Id);
                }
                catch (IOException e)
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