using Coinche.Server.Game.State;
using Lib.Sources;

namespace Coinche.Server
{
    public class Lobby
    {
        public AState State { get; private set; }
        public LobbyInfo Info { get; set; }

        public Lobby(LobbyInfo info)
        {
            Info = info;
            State = new WaitingState(this);
        }

        public Lobby(string name)
        {
            Info = new LobbyInfo(name);
            State = new WaitingState(this);
        }

        public void Update()
        {
            while (true)
            {
                if (!State.IsFinished()) return;
                State = State.NextState();
            }
        }

        public void AddClient(Client client)
        {
            Info.Clients.Add(client.Info);
            client.Lobby = this;
            
            // Update the Lobby
            Update();
        }

        public void RemoveClient(Client client)
        {
            Info.Clients.Remove(client.Info);
            client.Lobby = null;
            
            // Set to Default State
            State = new WaitingState(this);
        }

        public void HandleAction()
        {
            State.HandleAction();
            
            // Update the Lobby
            Update();
        }
    }
}