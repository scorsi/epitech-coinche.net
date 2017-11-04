using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Coinche.Server
{
    public class Client
    {
        /**
         * Socket
         */
        public TcpClient Socket { get; }

        private readonly NetworkStream _stream;
        
        /**
         * Attribute
         */
        public int Id { get; }

        public string Username { get; }

        /**
         * Thread
         */
        private Thread _thread;

        /**
         * Constructor
         */
        public Client(TcpClient socket, int id)
        {
            this.Socket = socket;
            this._stream = this.Socket.GetStream();
            this.Id = id;
            this.Username = this.Id.ToString();
        }

        /**
         * Initialize and start thread
         */
        public void Initialize()
        {
            this._thread = new Thread(this.Run);
            this._thread.Start();
        }

        /**
         * Run
         */
        private void Run()
        {
            var bytesFrom = new byte[1024];

            while (true)
            {
                try
                {
                    this._stream.Read(bytesFrom, 0, 1024);

                    var dataFromClient = Encoding.ASCII.GetString(bytesFrom);

                    Console.Out.WriteLineAsync("Client " + this.Id + " : " + dataFromClient);

                    Server.Singleton.Broadcast(dataFromClient, true, this);
                    Array.Clear(bytesFrom, 0, bytesFrom.Length);
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
            if (this._thread.IsAlive)
                this._thread.Abort();
            this.Socket.Close();
        }
    }
}