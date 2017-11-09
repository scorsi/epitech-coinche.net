using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Server.Game.State;
using Lib.Sources;

namespace Coinche.Server
{
    public class Lobby
    {
        private AState State { get; set; }
        public LobbyInfo Info { get; private set; }
        private List<Client> Clients { get; set; } = new List<Client>();

        public Lobby(LobbyInfo info)
        {
            Info = info;
            State = new WaitingState(this);
            State.Initialize();
        }

        public Lobby(string name)
        {
            Info = new LobbyInfo(name);
            State = new WaitingState(this);
            State.Initialize();
        }

        private void Update()
        {
            while (true)
            {
                if (!State.IsFinished()) return;
                State = State.NextState();
                State.Initialize();
            }
        }

        public void AddClient(Client client)
        {
            Clients.Add(client);
            Info.Clients.Add(client.Info);
            client.Lobby = this;
            
            // Update the Lobby State
            Update();
        }

        public void RemoveClient(Client client)
        {
            Clients.Remove(client);
            Info.Clients.Remove(client.Info);
            client.Lobby = null;
            
            // Check if we have to destroy Lobby
            if (Clients.Count == 0)
            {
                Server.Singleton.LobbyList.Remove(this);
                return; // Stop
            }
            
            // Set state to Waiting State if not already in
            if (!State.Name.Equals(WaitingState.DefaultName))
                State = new WaitingState(this);
        }

        public void HandleAction()
        {
            State.HandleAction();
            
            // Update the Lobby State
            Update();
        }
        
        /**
         * Broadcast to all connected clients
         */
        public void Broadcast(string msg, bool flag = false, Client client = null)
        {
            if (flag && client == null)
                return; // Add some security for NullReferenceException
            
            foreach (var broadcastClient in Clients)
            {
                if (broadcastClient == client)
                    continue; // Stop if it is the same socket

                NetworkStream broadcastStream;
                try
                {
                    broadcastStream = broadcastClient.Socket.GetStream();
                }
                catch (InvalidOperationException)
                {
                    continue;
                }

                Server.Singleton.WriteManager.Run(broadcastStream, Wrapper.Type.Message, 
                    (flag) 
                        ? (client.Info.Name + " says : " + msg)
                        : (msg));
            }
        }
    }
}