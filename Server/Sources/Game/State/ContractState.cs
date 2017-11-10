using Coinche.Protobuf;

namespace Coinche.Server.Game.State
{
    public class ContractState : AState
    {
        public ContractState(Lobby lobby) : base("Contract", lobby)
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