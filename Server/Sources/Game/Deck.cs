﻿using System.Collections.Generic;
using Lib.Game.Card;

namespace Coinche.Server.Game
{
    public class Deck
    {
        public List<Card> Cards { get; set; }

        public Deck()
        {
            Cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }
    }
}