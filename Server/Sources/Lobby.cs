using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Server.Game.State;
using Lib;
using Lib.Game.Card;

namespace Coinche.Server
{
    public class Lobby
    {
        private AState State { get; set; }
        public LobbyInfo Info { get; private set; }
        private List<Client> Clients { get; set; } = new List<Client>();
        public Dictionary<Team, int> Points { get; set; }

        public Lobby(LobbyInfo info)
        {
            Points = new Dictionary<Team, int>();
            Info = info;
            State = new WaitingState(this);
            State.Initialize();
        }

        public Lobby(string name)
        {
            Points = new Dictionary<Team, int>();
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
            Broadcast(client.Info.Name + " joined the lobby.");
            
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
            
            Broadcast(client.Info.Name + " left the lobby.");
            
            // Check if we have to destroy Lobby
            if (Clients.Count == 0)
            {
                Server.Singleton.LobbyList.Remove(this);
                return; // Stop
            }
            
            // Set state to Waiting State if not already in
            if (State.Name.Equals(WaitingState.DefaultName)) return;
            Broadcast("The game is over, waiting for players to join.");
            State = new WaitingState(this);
        }

        public bool HandleAction(Wrapper command, Client client)
        {
            var ret = false;
            try
            {
                ret = State.HandleAction(command, client);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLineAsync(e.ToString());
            }
            
            // Update the Lobby State
            Update();

            return ret;
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