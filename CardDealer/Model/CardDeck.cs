using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CardDealer.Model
{
    public class CardDeck : INotifyPropertyChanged
    {
        #region Local Variables
        Card[] _cardArray;
        bool _useJokers;
        private readonly object _cardsLock = new object();
        private readonly object _drawnLock = new object();

        Task _shuffle, _drawCard;

        #endregion

        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public ObservableCollection<Card> Cards { get; set; }
        public ObservableCollection<Card> DrawnCards { get; set; }
        public bool UseJokers
        {
            get
            {
                return _useJokers;
            }
            set
            {
                _useJokers = value;
                OnPropertyChanged("UseJokers");
            }
        }
        #endregion

        #region Constructor
        public CardDeck()
        {
            UseJokers = false;

            // Instantiate the collections
            Cards = new ObservableCollection<Card>();
            DrawnCards = new ObservableCollection<Card>();

            // Enabled collection synchronisation so that we can update the observable collections asynchronously
            BindingOperations.EnableCollectionSynchronization(Cards, _cardsLock);
            BindingOperations.EnableCollectionSynchronization(DrawnCards, _drawnLock);

            BeginShuffle();
        }
        #endregion

        #region Private Methods

        void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Populate the card array with a full set of playing cards, including jokers (non-random order)
        /// </summary>
        void PopulateCardArray()
        {
            _cardArray = new Card[54];

            int ix = 0;
            for (int i = 2; i <= 14; i++)
            {
                _cardArray[ix++] = new Card() { Value = i, Suit = Card.CardSuit.Hearts };
                _cardArray[ix++] = new Card() { Value = i, Suit = Card.CardSuit.Diamonds };
                _cardArray[ix++] = new Card() { Value = i, Suit = Card.CardSuit.Clubs };
                _cardArray[ix++] = new Card() { Value = i, Suit = Card.CardSuit.Spades };
            }

            _cardArray[ix++] = new Card() { Value = 0, Suit = Card.CardSuit.Joker };
            _cardArray[ix++] = new Card() { Value = 0, Suit = Card.CardSuit.Joker };
        }

        /// <summary>
        /// Shuffle the cards by creating a new card array in a randomized order and adding them to the Cards collection
        /// Also clear the Drawn Deck
        /// </summary>
        void Shuffle()
        {
            // Populate the card array (if needed)
            if (_cardArray == null) PopulateCardArray();

            // Create a new stack of cards in a random order
            Cards.Clear();

            // Clear the drawn cards also
            DrawnCards.Clear();

            // Create a shuffled card array from the full deck (or full deck minus jokers if UseJokers is false)
            Random rng = new Random();
            var shuffled = (UseJokers ? _cardArray : _cardArray.Where(x => x.Suit != Card.CardSuit.Joker)).OrderBy(x => rng.Next()).ToArray();

            // Push the cards onto the card stack
            for (int i = 0; i < shuffled.Length; i++)
            {
                Cards.Add(shuffled[i]);
                //Thread.Sleep(100); // Simulate slowness in the shuffling
            }
        }

        /// <summary>
        /// Draw a card from the Cards collection and place it in the DrawnCards
        /// </summary>
        void DrawCard()
        {
            //Thread.Sleep(500); // Simulate slowness in the card draw
            if (Cards.Count > 0)
            {
                var card = Cards[0];
                DrawnCards.Add(card);
                Cards.RemoveAt(0);
            }
        }
        #endregion

        #region Public Methods

        public async void BeginShuffle()
        {
            try
            {
                // Only allow one shuffle operation at once
                if (_shuffle == null || !_shuffle.Status.Equals(TaskStatus.Running)) _shuffle = new Task(() => { Shuffle(); });
                if (!_shuffle.Status.Equals(TaskStatus.Running)) { _shuffle.Start(); await _shuffle; }
            }
            catch (Exception ex)
            {
                // Should not be displaying message boxes from the model. Need to implement a message service at some point.
                MessageBox.Show($"{ex.Message}\r\n{ex.StackTrace}", "Card Dealer");
            }
        }

        public async void BeginDrawCard()
        {
            try
            {
                // Only allow one draw card operation at once
                if (_drawCard == null || !_drawCard.Status.Equals(TaskStatus.Running)) _drawCard = new Task(() => { DrawCard(); });
                if (!_drawCard.Status.Equals(TaskStatus.Running)) { _drawCard.Start(); await _drawCard; }
            }
            catch (Exception ex)
            {
                // Should not be displaying message boxes from the model. Need to implement a message service at some point.
                MessageBox.Show($"{ex.Message}\r\n{ex.StackTrace}", "Card Dealer");
            }
        }
        #endregion
    }
}
