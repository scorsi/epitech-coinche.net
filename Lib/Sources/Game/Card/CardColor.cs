using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Game.Card
{
    public class CardColor
    {
        public enum EColor
        {
            Undefined = 1,
            Club = 2,
            Diamond = 3,
            Heart = 4,
            Spade = 5
        }
        
        private static readonly Dictionary<EColor, CardColor> CardColorsMapping = new Dictionary<EColor, CardColor>();
        
        public static readonly CardColor Club = new CardColor(EColor.Club, "Club");
        public static readonly CardColor Diamond = new CardColor(EColor.Diamond, "Diamond");
        public static readonly CardColor Heart = new CardColor(EColor.Heart, "Heart");
        public static readonly CardColor Spade = new CardColor(EColor.Spade, "Spade");

        public EColor Index { get; }
        public string Name { get; }
        
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

        public static CardColor From(int id)
        {
            return CardColorsMapping[(EColor) id];
        }
        
        public static CardColor From(string name)
        {
            return (from color in CardColorsMapping where color.Value.Name.ToLower().Equals(name) select color.Value).FirstOrDefault();
        }
        
    }
}