using System.Collections.Generic;

namespace Lib.Game.Card
{
    public class CardFace
    {
        public enum EFace
        {
            Seven = 0,
            Eight = 1,
            Nine = 2,
            Ten = 3,
            Jack = 4,
            Queen = 5,
            King = 6,
            As = 7
        }

        private static readonly Dictionary<EFace, CardFace> CardFacesMapping = new Dictionary<EFace, CardFace>();
        
        public static readonly CardFace Seven = new CardFace(EFace.Seven, "7", 0, 0, 0, 0);
        public static readonly CardFace Eight = new CardFace(EFace.Eight, "8", 0, 0, 0, 0);
        public static readonly CardFace Nine = new CardFace(EFace.Nine, "9", 9, 0, 14, 0);
        public static readonly CardFace Ten = new CardFace(EFace.Ten, "10", 5, 10, 10, 10);
        public static readonly CardFace Jack = new CardFace(EFace.Jack, "Jack", 14, 2, 20, 2);
        public static readonly CardFace Queen = new CardFace(EFace.Queen, "Queen", 2, 3, 3, 3);
        public static readonly CardFace King = new CardFace(EFace.King, "King", 3, 4, 4, 4);
        public static readonly CardFace As = new CardFace(EFace.As, "As", 7, 19, 11, 11);
        
        private EFace Index { get; }
        public string Name { get; }
        public int PointAllTrump { get; }
        public int PointNoTrump { get; }
        public int PointOneTrump { get; }
        public int PointIsNotTrump { get; }

        private CardFace(EFace index, string name,
            int pointAllTrump, int pointNoTrump,
            int pointOneTrump, int pointIsNotTrump)
        {
            Index = index;
            Name = name;
            PointAllTrump = pointAllTrump;
            PointNoTrump = pointNoTrump;
            PointOneTrump = pointOneTrump;
            PointIsNotTrump = pointIsNotTrump;
            CardFacesMapping[index] = this;
        }

        public static CardFace From(EFace face)
        {
            return CardFacesMapping[face];
        }

        public static CardFace From(int id)
        {
            return CardFacesMapping[(EFace) id];
        }
    }
}
