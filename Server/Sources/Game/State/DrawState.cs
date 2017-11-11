using Coinche.Protobuf;

namespace Coinche.Server.Game.State
{
    public class DrawState : AState
    {

        private bool IsGenerated { get; set; } = false;
        
        public DrawState(Lobby lobby) : base("Draw", lobby)
        {
        }
        
        public override void Initialize()
        {
            Lobby.Broadcast("Distribution of cards.");
            var decks = DeckGenerator.GenerateAllDecks();
            foreach (var client in Lobby.Info.Clients)
            {
                // client.Deck = decks[0];
                decks.RemoveAt(0);
            }
            IsGenerated = true;
        }

        public override bool IsFinished()
        {
            return IsGenerated;
        }

        public override AState NextState()
        {
            return new ContractState(Lobby);
        }

        public override bool HandleAction(Wrapper command, Client client) => false;
    }
}