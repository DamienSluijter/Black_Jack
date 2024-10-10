using Black_Jack.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Black_Jack
{
    public partial class MainWindow : Window
    {
        private Game game;
        private DispatcherTimer timer;
        private List<Image> playerImages;
        private List<Image> dealerImages;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game(100);
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

            playerImages = new List<Image>
        {
            imgSpelerCard1, imgSpelerCard2, imgSpelerCard3,
            imgSpelerCard4, imgSpelerCard5, imgSpelerCard6, imgSpelerCard7
        };

            dealerImages = new List<Image>
        {
            imgBankCard1, imgBankCard2, imgBankCard3,
            imgBankCard4, imgBankCard5, imgBankCard6
        };
        }

        private void BtnDeel_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtStake.Text, out double bet) && bet <= game.Kapital)
            {
                btnDeel.IsEnabled = false;
                ResetCardImages();

                txtStake.IsEnabled = false;
                game.StartNewRound(bet);
                DealCards();
            }
            else
            {
                MessageBox.Show("Invalid bet or insufficient kapital.");
            }
        }

        private async void DealCards()
        {
            for (int i = 0; i < 2; i++)
            {
                DealCardToPlayer();
                UpdateUI();
                await Task.Delay(1000);
            }

            DealCardToDealer();
            UpdateUI();
            await Task.Delay(1000);

            if (!game.Player.IsBusted)
                EnablePlayerActions();
        }

        private void DealCardToPlayer()
        {
            game.Player.ReceiveCard(game.Deck.DealCard());
            UpdatePlayerCards();
        }

        private void DealCardToDealer()
        {
            game.Dealer.ReceiveCard(game.Deck.DealCard());
            UpdateDealerCards();
        }

        private void UpdatePlayerCards()
        {

            if (!game.DoubleDownActive)
            {
              for (int i = 0; i < game.Player.Hand.Count && i < playerImages.Count; i++)
              {
                 playerImages[i].Source = new BitmapImage(new Uri(game.Player.Hand[i].GetImagePath(), UriKind.RelativeOrAbsolute));
              }
            }

            else
            {
                int lastCardIndex = game.Player.Hand.Count - 1;
                imgSpelerCard7.Source = new BitmapImage(new Uri(game.Player.Hand[lastCardIndex].GetImagePath(), UriKind.RelativeOrAbsolute));
            }
        }

        private void UpdateDealerCards()
        {
            for (int i = 0; i < game.Dealer.Hand.Count && i < dealerImages.Count; i++)
            {
                dealerImages[i].Source = new BitmapImage(new Uri(game.Dealer.Hand[i].GetImagePath(), UriKind.RelativeOrAbsolute));
            }
        }

        private void ResetCardImages()
        {
            foreach (var img in playerImages.Concat(dealerImages))
            {
                img.Source = null;
            }
        }

        private void UpdateUI()
        {
            lblPlayerPoints.Content = game.Player.Points;
            lblDealerPoints.Content = game.Dealer.Points;
            lblCardsInDeck.Content = game.Deck.Cards.Count.ToString();

            if (game.Player.IsBusted)
            {
                EndRound();
            }
        }

        private async void DealerPlay()
        {
            while (game.Dealer.Points < 17)
            {
                game.DealerHit();
                UpdateDealerCards();
                UpdateUI();
                await Task.Delay(1000);
            }
            EndRound();
        }

        private void EnablePlayerActions()
        {
            btnHit.IsEnabled = true;
            btnStand.IsEnabled = true;
            btnDoubleDown.IsEnabled = game.CanDoubleDown;
        }

        private void DisablePlayerActions()
        {
            btnHit.IsEnabled = false;
            btnStand.IsEnabled = false;
            btnDoubleDown.IsEnabled = false;
        }

        private void BtnHit_Click(object sender, RoutedEventArgs e)
        {
            game.PlayerHit();
            UpdatePlayerCards();
            UpdateUI();
        }

        private void BtnStand_Click(object sender, RoutedEventArgs e)
        {
            DisablePlayerActions();
            DealerPlay();
        }

        private void BtnDoubleDown_Click(object sender, RoutedEventArgs e)
        {
            game.PlayerDoubleDown();
            UpdatePlayerCards();
            UpdateUI();

            if (!game.Player.IsBusted)
                DealerPlay();
        }

        private void EndRound()
        {
            DisablePlayerActions();

            string outcome = game.ResolveRound();
            txtKapital.Text = game.Kapital.ToString();
            MessageBox.Show(outcome);

            if (game.Kapital <= 0)
            {
                MessageBox.Show("You're out of kapital! Resetting the game.");
                btnReset.Visibility = Visibility.Visible;
            }
            else
            {
                btnDeel.IsEnabled = true;
                txtStake.IsEnabled = true;
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            btnReset.Visibility = Visibility.Collapsed;
            game.ResetGame();
            txtKapital.Text = game.Kapital.ToString();
            btnDeel.IsEnabled = true;
            txtStake.IsEnabled = true;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                button.BorderThickness = new Thickness(3);
                button.FontSize = 25;
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                button.BorderThickness = new Thickness(1);
                button.FontSize = 20;
            }
        }
    }
}