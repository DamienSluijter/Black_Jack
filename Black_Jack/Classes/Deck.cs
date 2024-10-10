using System;
using System.Collections.Generic;

namespace Black_Jack.Classes
{
    public class Deck
    {
        public List<Card> Cards;
        private Random Random = new Random();

        public Deck()
        {
            InitializeDeck();
        }

        private void InitializeDeck()
        {
            Cards = new List<Card>();
            string[] suits = { "Klaver", "Ruiten", "Harten", "Schoppen" };

            for (int i = 2; i <= 10; i++)
            {
                foreach (var suit in suits)
                {
                    Cards.Add(new Card($"{suit}{i}", i));
                }
            }

            AddFaceCards(suits);
        }

        private void AddFaceCards(string[] suits)
        {
            foreach (var suit in suits)
            {
                Cards.Add(new Card($"{suit}Boer", 10));
                Cards.Add(new Card($"{suit}Koningin", 10));
                Cards.Add(new Card($"{suit}Koning", 10));
                Cards.Add(new Card($"{suit}Aas", 11));
            }
        }

        public Card DealCard()
        {
            if (Cards.Count == 0)
            {
                InitializeDeck();
            }
            int index = Random.Next(Cards.Count);
            Card card = Cards[index];
            Cards.RemoveAt(index);
            return card;
        }
    }
}