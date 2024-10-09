using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace BlackJack
{
    public partial class Form1 : Form
    {
        // runs the program
        public Form1()
        {
            InitializeComponent();
            hitButton.Enabled = false;
            hitButton.Visible = false;
            standButton.Enabled = false;
            standButton.Visible = false;
            GenerateDeck();
            UpdateMoneyCount();
        }
        // Random object for dealing random cards
        Random random = new Random();

        // Variable to track money
        // Sets initial money at 100
        int playerMoney = 100;

        // Variable that lets user set the wager
        int playerBet { get; set; }

        // sum of player's point values
        int playerSum = 0;

        // number of aces dealt to the player
        int aceCount = 0;

        // List for tracking cards dealt to the player
        List<Card> PlayerHand = new List<Card>();

        // List for cards dealt to the dealer
        List<Card> DealerHand = new List<Card>();

        //List for the whole deck
        List<Card> Deck = new List<Card>();

        // Variable for storing dealer's
        // pocket card before reveal
        public Card pocketCard;

        // Blackjack marker
        public bool BlackJack = false;

        // audio variables
        private WaveOutEvent backgroundMusicPlayer;
        private AudioFileReader backgroundMusic;
        private bool bgPlaying = false;
        private CancellationTokenSource cancellationTokenSource;

        private void PlayBackgroundMusic(CancellationToken token)
        {
            try
            {
                // Load and play background music (looping)
                backgroundMusicPlayer = new WaveOutEvent();
                backgroundMusic = new AudioFileReader("MarleySound1.wav");

                backgroundMusicPlayer.Init(backgroundMusic);
                backgroundMusicPlayer.Play();

                while (!token.IsCancellationRequested && backgroundMusicPlayer.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100); // Keep looping until cancellation is requested
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing background music: " + ex.Message);
            }
        }

        private void StopBackgroundMusic()
        {
            if (backgroundMusicPlayer != null)
            {
                backgroundMusicPlayer.Stop();
                backgroundMusicPlayer.Dispose();
                backgroundMusic.Dispose();
            }

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }
        }

        // Play sound effects (non-blocking, can be played while background music is running)
        private void PlaySoundEffect(string soundFile)
        {
            try
            {
                // Play the sound effect without stopping previous ones
                WaveOutEvent soundEffectPlayer = new WaveOutEvent();
                AudioFileReader soundEffect = new AudioFileReader(soundFile);

                soundEffectPlayer.Init(soundEffect);
                soundEffectPlayer.Play();

                // Dispose of resources after the sound finishes playing
                soundEffectPlayer.PlaybackStopped += (s, e) =>
                {
                    soundEffectPlayer.Dispose();
                    soundEffect.Dispose();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing sound effect: " + ex.Message);
            }
        }

        private void musicCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (musicCheck.Checked && !bgPlaying)
            {
                // Start playing background music
                bgPlaying = true;
                cancellationTokenSource = new CancellationTokenSource();
                Task.Run(() => PlayBackgroundMusic(cancellationTokenSource.Token));
                musicCheck.Text = "Music on";
            }
            else
            {
                // Stop playing background music
                StopBackgroundMusic();
                bgPlaying = false;
                musicCheck.Text = "Music off";
            }

        }

        // Define card parameters
        public class Card
        {
            public string Suit { get; set; }
            public string Rank { get; set; }
            public int PointValue { get; set; }
            public Image CardImage { get; set; }
            public Card(string suit, string rank, int pointValue, Image cardImage)
            {
                Suit = suit;
                Rank = rank;
                PointValue = pointValue;
                CardImage = cardImage;
            }
        }

        // Defines card images
        Image kingHearts = Properties.Resources.kingHearts;
        Image queenHearts = Properties.Resources.queenHearts;
        Image jackHearts = Properties.Resources.jackHearts;
        Image tenHearts = Properties.Resources.tenHearts;
        Image nineHearts = Properties.Resources.nineHearts;
        Image eightHearts = Properties.Resources.eightHearts;
        Image sevenHearts = Properties.Resources.sevenHearts;
        Image sixHearts = Properties.Resources.sixHearts;
        Image fiveHearts = Properties.Resources.fiveHearts;
        Image fourHearts = Properties.Resources.fourHearts;
        Image threeHearts = Properties.Resources.threeHearts;
        Image twoHearts = Properties.Resources.twoHearts;
        Image aceHearts = Properties.Resources.aceHearts;

        Image kingDiamonds = Properties.Resources.kingDiamonds;
        Image queenDiamonds = Properties.Resources.queenDiamonds;
        Image jackDiamonds = Properties.Resources.jackDiamonds;
        Image tenDiamonds = Properties.Resources.tenDiamonds;
        Image nineDiamonds = Properties.Resources.nineDiamonds;
        Image eightDiamonds = Properties.Resources.eightDiamonds;
        Image sevenDiamonds = Properties.Resources.sevenDiamonds;
        Image sixDiamonds = Properties.Resources.sixDiamonds;
        Image fiveDiamonds = Properties.Resources.fiveDiamonds;
        Image fourDiamonds = Properties.Resources.fourDiamonds;
        Image threeDiamonds = Properties.Resources.threeDiamonds;
        Image twoDiamonds = Properties.Resources.twoDiamonds;
        Image aceDiamonds = Properties.Resources.aceDiamonds;

        Image kingClubs = Properties.Resources.kingClubs;
        Image queenClubs = Properties.Resources.queenClubs;
        Image jackClubs = Properties.Resources.jackClubs;
        Image tenClubs = Properties.Resources.tenClubs;
        Image nineClubs = Properties.Resources.nineClubs;
        Image eightClubs = Properties.Resources.eightClubs;
        Image sevenClubs = Properties.Resources.sevenClubs;
        Image sixClubs = Properties.Resources.sixClubs;
        Image fiveClubs = Properties.Resources.fiveClubs;
        Image fourClubs = Properties.Resources.fourClubs;
        Image threeClubs = Properties.Resources.threeClubs;
        Image twoClubs = Properties.Resources.twoClubs;
        Image aceClubs = Properties.Resources.aceClubs;

        Image kingSpades = Properties.Resources.kingSpades;
        Image queenSpades = Properties.Resources.queenSpades;
        Image jackSpades = Properties.Resources.jackSpades;
        Image tenSpades = Properties.Resources.tenSpades;
        Image nineSpades = Properties.Resources.nineSpades;
        Image eightSpades = Properties.Resources.eightSpades;
        Image sevenSpades = Properties.Resources.sevenSpades;
        Image sixSpades = Properties.Resources.sixSpades;
        Image fiveSpades = Properties.Resources.fiveSpades;
        Image fourSpades = Properties.Resources.fourSpades;
        Image threeSpades = Properties.Resources.threeSpades;
        Image twoSpades = Properties.Resources.twoSpades;
        Image aceSpades = Properties.Resources.aceSpades;

        Image cardBack = Properties.Resources.cardBack;

        // Create a deck, then populate cards
        public void GenerateDeck()
        {
            Deck.Add(new Card("Hearts", "King", 10, kingHearts));
            Deck.Add(new Card("Hearts", "Queen", 10, queenHearts));
            Deck.Add(new Card("Hearts", "Jack", 10, jackHearts));
            Deck.Add(new Card("Hearts", "Ten", 10, tenHearts));
            Deck.Add(new Card("Hearts", "Nine", 9, nineHearts));
            Deck.Add(new Card("Hearts", "Eight", 8, eightHearts));
            Deck.Add(new Card("Hearts", "Seven", 7, sevenHearts));
            Deck.Add(new Card("Hearts", "Six", 6, sixHearts));
            Deck.Add(new Card("Hearts", "Five", 5, fiveHearts));
            Deck.Add(new Card("Hearts", "Four", 4, fourHearts));
            Deck.Add(new Card("Hearts", "Three", 3, threeHearts));
            Deck.Add(new Card("Hearts", "Two", 2, twoHearts));
            Deck.Add(new Card("Hearts", "Ace", 11, aceHearts));

            Deck.Add(new Card("Diamonds", "King", 10, kingDiamonds));
            Deck.Add(new Card("Diamonds", "Queen", 10, queenDiamonds));
            Deck.Add(new Card("Diamonds", "Jack", 10, jackDiamonds));
            Deck.Add(new Card("Diamonds", "Ten", 10, tenDiamonds));
            Deck.Add(new Card("Diamonds", "Nine", 9, nineDiamonds));
            Deck.Add(new Card("Diamonds", "Eight", 8, eightDiamonds));
            Deck.Add(new Card("Diamonds", "Seven", 7, sevenDiamonds));
            Deck.Add(new Card("Diamonds", "Six", 6, sixDiamonds));
            Deck.Add(new Card("Diamonds", "Five", 5, fiveDiamonds));
            Deck.Add(new Card("Diamonds", "Four", 4, fourDiamonds));
            Deck.Add(new Card("Diamonds", "Three", 3, threeDiamonds));
            Deck.Add(new Card("Diamonds", "Two", 2, twoDiamonds));
            Deck.Add(new Card("Diamonds", "Ace", 11, aceDiamonds));

            Deck.Add(new Card("Clubs", "King", 10, kingClubs));
            Deck.Add(new Card("Clubs", "Queen", 10, queenClubs));
            Deck.Add(new Card("Clubs", "Jack", 10, jackClubs));
            Deck.Add(new Card("Clubs", "Ten", 10, tenClubs));
            Deck.Add(new Card("Clubs", "Nine", 9, nineClubs));
            Deck.Add(new Card("Clubs", "Eight", 8, eightClubs));
            Deck.Add(new Card("Clubs", "Seven", 7, sevenClubs));
            Deck.Add(new Card("Clubs", "Six", 6, sixClubs));
            Deck.Add(new Card("Clubs", "Five", 5, fiveClubs));
            Deck.Add(new Card("Clubs", "Four", 4, fourClubs));
            Deck.Add(new Card("Clubs", "Three", 3, threeClubs));
            Deck.Add(new Card("Clubs", "Two", 2, twoClubs));
            Deck.Add(new Card("Clubs", "Ace", 11, aceClubs));

            Deck.Add(new Card("Spades", "King", 10, kingSpades));
            Deck.Add(new Card("Spades", "Queen", 10, queenSpades));
            Deck.Add(new Card("Spades", "Jack", 10, jackSpades));
            Deck.Add(new Card("Spades", "Ten", 10, tenSpades));
            Deck.Add(new Card("Spades", "Nine", 9, nineSpades));
            Deck.Add(new Card("Spades", "Eight", 8, eightSpades));
            Deck.Add(new Card("Spades", "Seven", 7, sevenSpades));
            Deck.Add(new Card("Spades", "Six", 6, sixSpades));
            Deck.Add(new Card("Spades", "Five", 5, fiveSpades));
            Deck.Add(new Card("Spades", "Four", 4, fourSpades));
            Deck.Add(new Card("Spades", "Three", 3, threeSpades));
            Deck.Add(new Card("Spades", "Two", 2, twoSpades));
            Deck.Add(new Card("Spades", "Ace", 11, aceSpades));

            return;
        }

        //This reduces the player's bet from the player's available money
        //If player tries to bet zero, prompt for a wager
        //If player tries to bet more than they have, display error message
        public bool ValidateBet()
        {
            playerBet = Convert.ToInt32(Bet.Value);

            if (playerBet <= 0)
            {
                MessageBox.Show("Make a bet!");
                return false;

            }
            else if (playerMoney < playerBet)
            {
                MessageBox.Show("Not enough money!");
                return false;
            }

            ReduceFunds();
            return true;
        }
        // Reduces bet amount from playerMoney
        public void ReduceFunds()
        {
            playerMoney -= playerBet;
        }
        // Updates the Money count
        public void UpdateMoneyCount()
        {
            moneyCount.Text = ($"Funds: {playerMoney} $");
        }

        //This assigns functions to the "Deal" button
        //It hides the "deal" button and shows the "hit" and "stand" buttons
        private void dealButton_Click(object sender, EventArgs e)
        {
            if (!ValidateBet())
            {
                return;
            }
            Task.Run(() => PlaySoundEffect("Shuffle1.wav"));
            UpdateMoneyCount();
            dealCards();
            dealButton.Enabled = false;
            dealButton.Visible = false;
            hitButton.Enabled = true;
            hitButton.Visible = true;
            standButton.Enabled = true;
            standButton.Visible = true;
        }
        //This randomly picks the first set of four cards
        public void dealCards()
        {
            int firstP = random.Next(Deck.Count);
            Card firstPlayerCard = Deck[firstP];
            PlayerHand.Add(firstPlayerCard);
            Deck.RemoveAt(firstP);
            playerFirst.Image = firstPlayerCard.CardImage;

            int secondP = random.Next(Deck.Count);
            Card secondPlayerCard = Deck[secondP];
            PlayerHand.Add(secondPlayerCard);
            Deck.RemoveAt(secondP);
            playerSecond.Image = secondPlayerCard.CardImage;

            playerSum = PlayerHand.Sum(card => card.PointValue);

            int firstD = random.Next(Deck.Count);
            Card firstDealerCard = Deck[firstD];
            DealerHand.Add(firstDealerCard);
            Deck.RemoveAt(firstD);
            dealerFirst.Image = firstDealerCard.CardImage;

            int secondD = random.Next(Deck.Count);
            Card secondDealerCard = Deck[secondD];
            DealerHand.Add(secondDealerCard);
            pocketCard = secondDealerCard;
            Deck.RemoveAt(secondD);
            dealerSecond.Image = cardBack;
        }

        //this deals additional cards to the player
        public void Hit()
        {
            //deals another card to player
            int hitMe = random.Next(Deck.Count);
            Card newPlayerCard = Deck[hitMe];
            PlayerHand.Add(newPlayerCard);
            Deck.RemoveAt(hitMe);

            //finds next empty box and adds the card
            PictureBox emptyBox = EmptyBoxPlayer();
            if (emptyBox != null)
            {
                emptyBox.Image = newPlayerCard.CardImage;
            }

            //Checks the sum of the player's hand
            playerSum = 0;
            foreach (Card card in PlayerHand)
            {
                playerSum += card.PointValue;
            }
            if (playerSum > 21)
            {
                Task.Run(() => PlaySoundEffect("FailSound2.wav"));
                MessageBox.Show("Bust! You lose!");
                ClearTable();
                hitButton.Enabled = false;
                hitButton.Visible = false;
                standButton.Enabled = false;
                standButton.Visible = false;
                dealButton.Enabled = true;
                dealButton.Visible = true;
                return;
            }
            else if (playerSum == 21)
            {
                MessageBox.Show("Blackjack!");
                BlackJack = true;
                Stand();
                return;
            }
        }
        public void dealerHit()
        {
            int newD = random.Next(Deck.Count);
            Card newDealerCard = Deck[newD];
            DealerHand.Add(newDealerCard);
            Deck.RemoveAt(newD);

            PictureBox emptyBox = EmptyBoxDealer();
            if (emptyBox != null)
            {
                emptyBox.Image = newDealerCard.CardImage;
            }
        }

        //This assigns function to the Hit button
        private void hitButton_Click(object sender, EventArgs e)
        {
            Task.Run(() => PlaySoundEffect("cardsound4.wav"));
            Hit();
            // Play the card sound when the hitButton is clicked
        }

        // Finds first empty player box
        private PictureBox EmptyBoxPlayer()
        {
            // Lists all available player boxes
            //
            List<PictureBox> playerBoxes = new List<PictureBox>
            {
                playerFirst,
                playerSecond,
                playerThird,
                playerFourth,
                playerFifth,
                playerSixth,
                playerSeventh,
                playerEighth
            };
            foreach (PictureBox box in playerBoxes)
            {
                if (box.Image == null)
                {
                    return box;
                }
            }
            //returns null if all eight boxes are full
            MessageBox.Show("Eight is plenty!");
            return null;
        }

        //finds first empty dealer box
        private PictureBox EmptyBoxDealer()
        {
            List<PictureBox> dealerBoxes = new List<PictureBox>
            {
                dealerFirst,
                dealerSecond,
                dealerThird,
                dealerFourth,
                dealerFifth,
                dealerSixth,
                dealerSeventh,
                dealerEighth
            };

            foreach (PictureBox box in dealerBoxes)
            {
                if (box.Image == null)
                {
                    return box;
                }
            }
            MessageBox.Show("No more!");
            return null;
        }

        // Resets all picture boxes and controls
        // resets the deck
        private void ClearTable()
        {
            List<PictureBox> allBoxes = new List<PictureBox>
            {
                dealerFirst,
                dealerSecond,
                dealerThird,
                dealerFourth,
                dealerFifth,
                dealerSixth,
                dealerSeventh,
                dealerEighth,
                playerFirst,
                playerSecond,
                playerThird,
                playerFourth,
                playerFifth,
                playerSixth,
                playerSeventh,
                playerEighth
            };

            foreach (PictureBox box in allBoxes)
            {
                box.Image = null;
            }
            PlayerHand.Clear();
            DealerHand.Clear();
            Deck.Clear();

            GenerateDeck();
            BlackJack = false;
            playerSum = 0;
            aceCount = 0;
            dealerSum = 0;
            hitButton.Enabled = false;
            hitButton.Visible = false;
            standButton.Enabled = false;
            standButton.Visible = false;
            dealButton.Enabled = true;
            dealButton.Visible = true;
            UpdateMoneyCount();
        }
        // Stand button functionality
        private void standButton_Click(object sender, EventArgs e)
        {
            Stand();
        }

        // Variable for checking dealer's total
        int dealerSum = 0;
        private void Stand()
        {
            dealerSecond.Image = pocketCard.CardImage;

            //Checks the sum of the dealer's hand

            {
                dealerSum = DealerHand.Sum(card => card.PointValue);

                if (dealerSum > 21)
                {
                    Task.Run(() => PlaySoundEffect("WINsound1.wav"));
                    MessageBox.Show("House busts! \n You win!");
                    PayOut();
                    ClearTable();
                    return;
                }
                //dealer draws a card, repeat until over 16
                while (dealerSum <= 16)
                {
                    dealerHit();
                    dealerSum += DealerHand.Last().PointValue;

                    if (dealerSum > 21)
                    {
                        Task.Run(() => PlaySoundEffect("WINsound1.wav"));
                        MessageBox.Show("House busts! \n You win!");
                        PayOut();
                        ClearTable();
                        return;
                    }
                }
                CompareHands();
                ClearTable();
                return;
            }
        }

        public void CompareHands()
        {
            if (playerSum > dealerSum)
            {
                Task.Run(() => PlaySoundEffect("WINsound1.wav"));
                MessageBox.Show("You Win!");
                PayOut();
            }
            else if (playerSum == dealerSum)
            {
                MessageBox.Show("Draw!");
                playerMoney += playerBet;
            }
            else
            {
                Task.Run(() => PlaySoundEffect("FailSound2.wav"));
                MessageBox.Show("House wins!");
                ClearTable();
            }

        }

        // pays winnings to player
        private void PayOut()
        {
            if (BlackJack == true)
            {
                playerMoney += (playerBet * 3);
                ClearTable();
            }
            else
            {
                playerMoney += (playerBet * 2);
            }
        }


    }
}