using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Game.Card;

namespace Coinche.Server.Game
{
    public class DeckGenerator
    {
        private static Random Random { get; } = new Random();
        
        public static List<Deck> GenerateAllDecks() {
            var decks = new List<Deck>();

            for (var i = 0; i < 4; ++i)
            {
                decks.Add(GenerateOneDeck(decks));
            }
            return decks;
        }
        
        private static bool IsCardAvailable(List<Deck> pastDecks, Deck actualDeck, Card cardToCheck)
        {
            return !pastDecks.SelectMany(deck => deck.Cards).Any(card => card.Info.ColorId == cardToCheck.Info.ColorId && card.Info.FaceId == cardToCheck.Info.FaceId) && actualDeck.Cards.All(card => card.Info.ColorId != cardToCheck.Info.ColorId || card.Info.FaceId != cardToCheck.Info.FaceId);
        }
        
        private static Deck GenerateOneDeck(List<Deck> pastDecks) {
            var deck = new Deck();
            for (var i = 0; i < 8; ++i) {
                Card card = null;
                var isAvailable = false;
                while (!isAvailable) {

                    var faceId = Random.Next(2, 10);
                    var colorId = Random.Next(2, 6);
                    var face = CardFace.From(faceId);
                    var color = CardColor.From(colorId);

                    card = new Card(face, color);
                
                    isAvailable = IsCardAvailable(pastDecks, deck, card);
                }
                deck.AddCard(card);
            }
            return deck;
        }
    }
}