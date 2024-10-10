using System.Collections.Generic;

namespace Black_Jack.Classes
{
    public class Player
    {
        public List<Card> Hand { get; private set; }
        public int Points { get; private set; }
        private int AceCount { get; set; }

        public Player()
        {
            Hand = new List<Card>();
            Points = 0;
            AceCount = 0;
        }

        public void ReceiveCard(Card card)
        {
            Hand.Add(card);
            Points += card.Value;
            if (card.IsAce)
            {
                AceCount++;
            }
            AdjustForAces();
        }

        public bool IsBusted => Points > 21;
        public bool HasBlackjack => Points == 21;

        private void AdjustForAces()
        {
            while (Points > 21 && AceCount > 0)
            {
                Points -= 10;
                AceCount--;
            }
        }

        public void Reset()
        {
            Hand.Clear();
            Points = 0;
            AceCount = 0;
        }
    }
}