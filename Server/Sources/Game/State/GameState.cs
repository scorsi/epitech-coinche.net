using Coinche.Protobuf;

namespace Coinche.Server.Game.State
{
    public class GameState : AState
    {
        public GameState(Lobby lobby) : base("Game", lobby)
        {
        }

        public override void Initialize()
        {
        }

        public override bool IsFinished()
        {
            return false;
        }

        public override AState NextState()
        {
            return null;
        }

        public override void HandleAction(Wrapper command, Client client)
        {
        }
    }
}