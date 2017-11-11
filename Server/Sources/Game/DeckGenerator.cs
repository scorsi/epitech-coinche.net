using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Game.Card;

namespace Coinche.Server.Game
{
    public class DeckGenerator
    {
        private Random Random { get; }

        public DeckGenerator()
        {
            Random = new Random(Environment.TickCount);
        }
        
        public List<Deck> GenerateAllDecks() {
            var decks = new List<Deck>();
            
            for (var i = 0; i < 4; ++i) {
                decks.Add(GenerateOneDeck(decks));
            }
            return decks;
        }
        
        private static bool IsCardAvailable(List<Deck> pastDecks, Deck actualDeck, Card cardToCheck)
        {
            return pastDecks.SelectMany(deck => deck.Cards).Any(card => card == cardToCheck) || actualDeck.Cards.Any(card => card == cardToCheck);
        }
        
        private Deck GenerateOneDeck(List<Deck> pastDecks) {
            Console.Out.WriteLine("Generate One Deck");
            
            var deck = new Deck();
            for (var i = 0; i < 8; ++i) {
                Console.Out.WriteLine("Generate One Card");

                Card card = null;
                var isAvailable = true;
                while (isAvailable) {
                    Console.Out.WriteLine("IsAvailable");

                    try
                    {
                        card = new Card(
                            CardFace.From(Random.Next(1, 8)),
                            CardColor.From(Random.Next(1, 4)));
                    
                        isAvailable = IsCardAvailable(pastDecks, deck, card);
                    }
                    catch (Exception e)
                    {
                        Console.Out.WriteLine(e.ToString());
                    }
                }
                deck.AddCard(card);
            }
            return deck;
        }
    }
}