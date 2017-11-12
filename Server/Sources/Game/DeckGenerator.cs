using System;
using System.Collections.Generic;
using System.Linq;
using Lib;
using Lib.Game.Card;

namespace Coinche.Server.Game
{
    public class DeckGenerator
    {
        private static Random Random { get; } = new Random();
        
        public static List<List<CardInfo>> GenerateAllDecks() {
            var decks = new List<List<CardInfo>>();

            for (var i = 0; i < 4; ++i)
            {
                decks.Add(GenerateOneDeck(decks));
            }
            return decks;
        }
        
        private static bool IsCardAvailable(List<List<CardInfo>> pastDecks, List<CardInfo> actualDeck, CardInfo cardToCheck)
        {
            return !pastDecks.SelectMany(deck => deck).Any(card => card.ColorId == cardToCheck.ColorId && card.FaceId == cardToCheck.FaceId) 
                   && actualDeck.All(card => card.ColorId != cardToCheck.ColorId || card.FaceId != cardToCheck.FaceId);
        }
        
        private static List<CardInfo> GenerateOneDeck(List<List<CardInfo>> pastDecks) {
            var deck = new List<CardInfo>();
            for (var i = 0; i < 8; ++i) {
                CardInfo card = null;
                var isAvailable = false;
                while (!isAvailable) {

                    var faceId = Random.Next(2, 10);
                    var colorId = Random.Next(2, 6);
                    var face = CardFace.From(faceId);
                    var color = CardColor.From(colorId);

                    card = new CardInfo {Face = face, Color = color};
                
                    isAvailable = IsCardAvailable(pastDecks, deck, card);
                }
                deck.Add(card);
            }
            return deck;
        }
    }
}