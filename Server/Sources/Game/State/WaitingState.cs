using Coinche.Protobuf;

namespace Coinche.Server.Game.State
{
    public class WaitingState : AState
    {
        public static string DefaultName { get; } = "Waiting";
        
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

        public override void HandleAction(Wrapper command, Client client)
        {
        }
    }
}