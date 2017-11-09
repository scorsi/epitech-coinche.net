namespace Coinche.Server.Game.State
{
    public abstract class AState
    {
        public Lobby Lobby { get; }
        public string Name { get; }

        public AState(string name, Lobby lobby)
        {
            Name = name;
            Lobby = lobby;
        }
        
        public abstract void Initialize();

        public abstract bool IsFinished();

        public abstract AState NextState();

        public abstract void HandleAction();
    }
}