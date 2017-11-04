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
            Socket = socket;
            _stream = Socket.GetStream();
            Id = id;
            Username = Id.ToString();
        }

        /**
         * Initialize and start thread
         */
        public void Initialize()
        {
            _thread = new Thread(Run);
            _thread.Start();
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
                    if (_stream.Read(header, 0, 2) != 2) continue;
                    var type = (Wrapper.Type) BitConverter.ToInt16(header, 0);
                    switch (type)
                    {
                        case Wrapper.Type.Message:
                            var message = ProtoBuf.Serializer.DeserializeWithLengthPrefix<Message>(_stream, ProtoBuf.PrefixStyle.Fixed32);
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
            if (_thread.IsAlive)
                _thread.Abort();
            Socket.Close();
        }
    }
}