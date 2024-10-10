using System;

namespace Black_Jack.Classes
{
    public class Game
    {
        public Player Player { get; private set; }
        public Player Dealer { get; private set; }
        public Deck Deck { get; private set; }
        public double Kapital { get; private set; }
        public double Bet { get; set; }

        public bool DoubleDownActive;

        public Game(double initialKapital)
        {
            Player = new Player();
            Dealer = new Player();
            Deck = new Deck();
            Kapital = initialKapital;
        }

        public void StartNewRound(double bet)
        {
            if (bet > Kapital)
                throw new InvalidOperationException("Bet exceeds available kapital!");

            Bet = bet;
            DoubleDownActive = false;
            Player.Reset();
            Dealer.Reset();
        }

        public void PlayerHit()
        {
            Player.ReceiveCard(Deck.DealCard());
        }

        public void DealerHit()
        {
            Dealer.ReceiveCard(Deck.DealCard());
        }

        public string ResolveRound()
        {
            if (Player.IsBusted)
            {
                Kapital -= Bet;
                return "Player Loses! (Busted)";
            }
            else if (Dealer.IsBusted)
            {
                Kapital += Bet;
                return "Player Wins! (Dealer Busted)";
            }
            else if (Player.Points > Dealer.Points)
            {
                Kapital += Bet;
                return "Player Wins!";
            }
            else if (Player.Points < Dealer.Points)
            {
                Kapital -= Bet;
                return "Player Loses!";
            }
            else
            {
                return "Push! (Draw)";
            }
        }

        public void PlayerDoubleDown()
        {
            Bet *= 2;
            DoubleDownActive = true;
            PlayerHit();
        }

        public bool CanDoubleDown => Bet * 2 <= Kapital;

        public void ResetGame()
        {
            Kapital = 100;
        }
    }
}