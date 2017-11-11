namespace Lib.Game.Card
{
    public class Card
    {
        public CardFace Face { get; }
        public CardColor Color { get; }
 
        public Card(CardFace face, CardColor color) {
            Face = face;
            Color = color;
        }
 
        public int GetPointAllTrump() {
            return Face.PointAllTrump;
        }
 
        public int GetPointNoTrump() {
            return Face.PointNoTrump;
        }
 
        public int GetPointOneTrump() {
            return Face.PointOneTrump;
        }
 
        public int GetPointIsNotTrump() {
            return Face.PointIsNotTrump;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (typeof(object) != typeof(Card)) return false;
            
            var other = (Card) obj;
            return Face.Index == other.Face.Index && Color.Index == other.Color.Index;
        }
    }
}