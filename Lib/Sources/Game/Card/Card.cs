namespace Lib.Game.Card
{
    public class Card
    {
        public CardInfo Info { get; set; } = new CardInfo();

        public Card(CardInfo info)
        {
            Info = info;
        }
        
        public Card(CardFace face, CardColor color) {
            Info.Face = face;
            Info.Color = color;
        }
 
        public int GetPointAllTrump() {
            return Info.Face.PointAllTrump;
        }
 
        public int GetPointNoTrump() {
            return Info.Face.PointNoTrump;
        }
 
        public int GetPointOneTrump() {
            return Info.Face.PointOneTrump;
        }
 
        public int GetPointIsNotTrump() {
            return Info.Face.PointIsNotTrump;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (typeof(object) != typeof(Card)) return false;
            
            var other = (Card) obj;
            return Info.Face.Index == other.Info.Face.Index && Info.Color.Index == other.Info.Color.Index;
        }
    }
}