namespace Lib.Sources.Game.Card
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
    }
}