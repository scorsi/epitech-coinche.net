using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
        private readonly TcpClient _socket = new TcpClient();
        private NetworkStream _stream = default(NetworkStream);
        
        /**
         * Threads
         */
        private Thread _readThread;
        private Thread _writeThread;

        /**
         * Serve to known if the client has been properly initialized
         */
        public bool IsInitialized { get; private set; }

        /**
         * Default constructor
         */
        private Client()
        {
            this.Initialize(DefaultServerIp, DefaultServerPort);
        }

        /**
         * Constructor with custom parameters
         */
        private Client(string ip, int port)
        {
            this.Initialize(ip, port);
        }

        /**
         * Initialize method which will create the socket
         */
        public bool Initialize(string ip, int port)
        {
            this.IsInitialized = false;
            try
            {
                this._socket.Connect(ip, port);
                this._stream = this._socket.GetStream();
                this.IsInitialized = true;
                return true;
            }
            catch (Exception e)
            {
                Console.Out.WriteLineAsync(e.ToString());
                return false;
            }
        }

        /**
         * Run two different threads, one for read and the second for write
         */
        public bool Run()
        {
            try
            {
                this._readThread = new Thread(this.ThreadedReadRun);
                this._writeThread = new Thread(this.ThreadedWriteRun);

                this._readThread.Start();
                this._writeThread.Start();
                
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
            while (this.IsAlive())
            {
                try
                {
                    var bytesFrom = new byte[1024];
                    this._stream.Read(bytesFrom, 0, 1024);
                    var dataFromServer = Encoding.ASCII.GetString(bytesFrom);
                    Console.Out.WriteLineAsync(dataFromServer);
                }
                catch (Exception e)
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
            while (this.IsAlive())
            {
                try
                {
                    var outStream = Encoding.ASCII.GetBytes(Console.In.ReadLine());
                    this._stream.Write(outStream, 0, outStream.Length);
                    this._stream.FlushAsync();
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }
        
        /**
         * Serve to know of threads and socket are still alive
         */
        public bool IsAlive()
        {
            return this._readThread.IsAlive && this._writeThread.IsAlive && this._socket.Connected;
        }

        /**
         * Clear socket and threads
         */
        public void Clear()
        {
            if (this._readThread.IsAlive)
                this._readThread.Abort();
            
            if (this._writeThread.IsAlive)
                this._writeThread.Abort();
            
            this._socket.Close();
        }
        
    }
}