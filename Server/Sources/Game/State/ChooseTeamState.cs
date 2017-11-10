namespace Coinche.Server.Game.State
{
    public class ChooseTeamState : AState
    {
        public ChooseTeamState(Lobby lobby) : base("ChooseTeam", lobby)
        {
        }

        public override void Initialize()
        {
            System.Console.Out.WriteLineAsync("ChooseTeam");
        }

        public override bool IsFinished()
        {
            return false;
        }

        public override AState NextState()
        {
            return null;
        }

        public override void HandleAction()
        {
        }
    }
}