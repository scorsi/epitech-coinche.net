namespace Lib.Sources.Game.Card
{
    public class Player
    {
        private Team Team { get; set; }
        private string Name { get; set; }
        private Deck Deck { get; set; }

        public Player(string name) {
            Name = name;
            Deck = null;
        }
    }
}