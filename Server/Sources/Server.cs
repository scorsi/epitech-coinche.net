using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Coinche.Server
{
    internal class Server
    {
        public static void Main(string[] args)
        {
            if (!Server.Singleton.Initialize()) return;
            Server.Singleton.Run();
            Server.Singleton.Clear();
        }

        private const int DefaultServerPort = 8888;
        
        /**
         * Singleton
         */
        public static Server Singleton { get; } = new Server();

        /**
         * Socket Listener
         */
        private TcpListener _listener;

        /**
         * List of connected clients
         */
        public Hashtable ClientList { get; private set; }

        /**
         * Temporary list for clients to be removed
         */
        private readonly List<Client> _pendingDisconnection = new List<Client>();
        
        /**
         * Status attributes
         */
        public bool IsInitialized { get; private set; }
        public bool IsRunning { get; private set; }

        /**
         * Initiliaze listener
         */
        private bool Initialize(int port = DefaultServerPort)
        {
            this.IsInitialized = false;
            this.IsRunning = false;
            try
            {
                this._listener = new TcpListener(IPAddress.Any, port);
                this.ClientList = new Hashtable();

                this._listener.Start();
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
         * Run the server
         */
        private void Run()
        {
            if (!this.IsInitialized)
                return;

            this.IsRunning = true;

            var nb = 0;
            while (true)
            {
                var clientSocket = this._listener.AcceptTcpClient();
                ++nb;

                Console.Out.WriteLineAsync("Client " + nb + " joined the server.");

                var client = new Client(clientSocket, nb);
                client.Initialize();
                this.ClientList.Add(nb, client);
                
                // Check disconnection after all new clients
                this.CheckDisconnection();
            }
        }

        /**
         * Handle pending client disconnection
         */
        private void CheckDisconnection()
        {
            foreach (var client in this._pendingDisconnection)
            {
                client.Clear();
                Console.Out.WriteLineAsync("Client " + client.Id + " has disconnected.");
                this.ClientList.Remove(client.Id);
            }
        }

        /**
         * Clear everything
         */
        private void Clear()
        {
            this._pendingDisconnection.Clear();
            foreach (DictionaryEntry item in this.ClientList)
            {
                var client = (Client) item.Value;
                client.Clear();
                Console.Out.WriteLineAsync("Client " + client.Id + " has disconnected.");
            }
            this.ClientList.Clear();
            this._listener.Stop();
            this.IsInitialized = false;
            this.IsRunning = false;
        }

        /**
         * Broadcast to all connected clients
         */
        public void Broadcast(string msg, bool flag = false, Client client = null)
        {
            if (flag && client == null)
                return; // Add some security for NullReferenceException
            
            foreach (DictionaryEntry item in this.ClientList)
            {
                var broadcastClient = (Client) item.Value;
                if (!flag && broadcastClient == client)
                    continue; // Stop if it is the same socket

                var broadcastStream = default(NetworkStream);
                try
                {
                    broadcastStream = broadcastClient.Socket.GetStream();
                }
                catch (InvalidOperationException e)
                {
                    // Remove client if we can't get its stream
                    this._pendingDisconnection.Add(broadcastClient);
                    continue;
                }
                
                byte[] broadcastBytes = null;

                broadcastBytes = (flag == true)
                    ? (Encoding.ASCII.GetBytes(client.Username + " says : " + msg))
                    : (Encoding.ASCII.GetBytes(msg));
                
                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.FlushAsync();
            }
            
            // Check disconnection after all broadcast
            this.CheckDisconnection();
        }

        /**
         * Remove a client
         */
        public void RemoveClient(Client client)
        {
            this._pendingDisconnection.Add(client);
        }
        
    }
}