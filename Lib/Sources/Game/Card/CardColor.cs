using System.Collections.Generic;

namespace Lib.Sources.Game.Card
{
    public class CardColor
    {
        public enum EColor
        {
            Club = 0,
            Diamond = 1,
            Heart = 2,
            Spade = 3
        }
        
        private static readonly Dictionary<EColor, CardColor> CardColorsMapping = new Dictionary<EColor, CardColor>();
        
        public static readonly CardColor Club = new CardColor(EColor.Club, "Club");
        public static readonly CardColor Diamond = new CardColor(EColor.Diamond, "Diamond");
        public static readonly CardColor Heart = new CardColor(EColor.Heart, "Heart");
        public static readonly CardColor Spade = new CardColor(EColor.Spade, "Spade");

        private EColor Index { get; }
        private string Name { get; }
        
        private CardColor(EColor index, string name)
        {
            Index = index;
            Name = name;
            CardColorsMapping[index] = this;
        }

        public static CardColor From(EColor color)
        {
            return CardColorsMapping[color];
        }
    }
}