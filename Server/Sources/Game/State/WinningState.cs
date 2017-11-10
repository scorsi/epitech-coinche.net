using Coinche.Protobuf;

namespace Coinche.Server.Game.State
{
    public class WinningState : AState

    {
        public WinningState(string name, Lobby lobby) : base(name, lobby)
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

        public override void HandleAction(Wrapper command, Client client)
        {
        }
    }
}