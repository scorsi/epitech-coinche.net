using Coinche.Protobuf;
using Lib.Game.Card;

namespace Coinche.Server.Game.State
{
    public class ContractState : AState
    {
        private int Turn { get; set; }
        private Contract Contract { get; set; }
        
        public ContractState(Lobby lobby) : base("Contract", lobby)
        {
        }

        public override void Initialize()
        {
            Lobby.Broadcast("You can now put contracts.");
            Turn = 0;
            Contract = null;
        }

        public override bool IsFinished()
        {
            return Turn >= 4;
        }

        public override AState NextState()
        {
            if (Contract != null)
                return new GameState(Lobby);
            Lobby.Broadcast("Anyone put contract. Replay the round.");
            return new DrawState(Lobby);
        }

        public override bool HandleAction(Wrapper command, Client client)
        {
            if (command.ProtobufType != Wrapper.Type.LobbyContract) return false;
            System.Console.Out.WriteLine("Yeah");
            return true;
        }
    }
}