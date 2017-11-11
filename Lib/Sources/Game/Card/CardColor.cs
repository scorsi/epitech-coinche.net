using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Game.Card
{
    public class CardColor
    {
        public enum EColor
        {
            Club = 1,
            Diamond = 2,
            Heart = 3,
            Spade = 4
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
            foreach (var color in CardColorsMapping)
            {
                Console.Out.WriteLine(name + " vs " + color.Value.Name.ToLower());
                if (color.Value.Name.ToLower().Equals(name))
                    return color.Value;
            }
            return null;
        }
        
    }
}