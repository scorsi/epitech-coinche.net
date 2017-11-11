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
            
            for (var i = 0; i < 4; ++i) {
                decks.Add(GenerateOneDeck(decks));
            }
            return decks;
        }
        
        private static bool IsCardAvailable(List<Deck> pastDecks, Deck actualDeck, Card cardToCheck)
        {
            return !(pastDecks.SelectMany(deck => deck.Cards).Any(card => card.Equals(cardToCheck)) || actualDeck.Cards.Any(card => card.Equals(cardToCheck)));
        }
        
        private static Deck GenerateOneDeck(List<Deck> pastDecks) {
            Console.Out.WriteLine("Generate One Deck");
            
            var deck = new Deck();
            for (var i = 0; i < 8; ++i) {
                Console.Out.WriteLine("Generate One Card");

                Card card = null;
                var isAvailable = false;
                while (!isAvailable) {

                    var faceId = Random.Next(1, 8);
                    var colorId = Random.Next(1, 4);
                    var face = CardFace.From(faceId);
                    var color = CardColor.From(colorId);

                    Console.Out.WriteLine("IsAvailable " + faceId + " " + colorId);

                    card = new Card(face, color);
                
                    isAvailable = IsCardAvailable(pastDecks, deck, card);
                }
                deck.AddCard(card);
            }
            return deck;
        }
    }
}