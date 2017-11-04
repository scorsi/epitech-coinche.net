using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
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
            while (true)
            {
                try
                {
                    var header = new byte[2];
                    if (this._stream.Read(header, 0, 2) != 2) continue;
                    var type = (Wrapper.Type) BitConverter.ToInt16(header, 0);
                    switch (type)
                    {
                        case Wrapper.Type.Message:
                            var message = ProtoBuf.Serializer.DeserializeWithLengthPrefix<Message>(this._stream, ProtoBuf.PrefixStyle.Fixed32);
                            Console.Out.WriteLineAsync("Received message from client " + Id + " : " + message.Text);
                            break;
                        default:
                            Console.Out.WriteLineAsync("Received invalid data from client " + Id);
                            break;
                    }
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