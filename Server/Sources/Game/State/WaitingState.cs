namespace Coinche.Server.Game.State
{
    public class WaitingState : AState
    {
        public WaitingState(Lobby lobby) : base("Waiting", lobby)
        {
        }

        public override void Initialize()
        {
        }

        public override bool IsFinished()
        {
            return Lobby.Info.Clients.Count == 4;
        }

        public override AState NextState()
        {
            return new ChooseTeamState(Lobby);
        }

        public override void HandleAction()
        {
        }
    }
}