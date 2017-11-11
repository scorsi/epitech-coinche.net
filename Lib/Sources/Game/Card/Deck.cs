using System.Collections.Generic;

namespace Lib.Game.Card
{
    public class Deck
    {
        private List<Card> Cards { get; } = new List<Card>();

        public int FoundCard(CardFace face, CardColor color)
        {
            var i = 0;
            foreach (var card in Cards)
            {
                if (card.Info.Face.Equals(face) && card.Info.Color.Equals(color))
                {
                    return i;
                }
                ++i;
            }
            return -1;
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

    }}