namespace Lib.Sources.Game.Card
{
    public class Card
    {
        private CardFace face;
        private CardColor color;

        Card(CardFace face, CardColor color) {
            this.face = face;
            this.color = color;
        }

        public CardFace getFace() {
            return this.face;
        }

        public CardColor getColor() {
            return this.color;
        }

        public string getColorName() {
            return this.getColor().Name;
        }

        public string getFaceName() {
            return this.getFace().Name;
        }

        public int getPointAllTrump() {
            return this.getFace().PointAllTrump;
        }

        public int getPointNoTrump() {
            return this.getFace().PointNoTrump;
        }

        public int getPointOneTrump() {
            return this.getFace().PointOneTrump;
        }

        public int getPointIsNotTrump() {
            return this.getFace().PointIsNotTrump;
        }
    }
}