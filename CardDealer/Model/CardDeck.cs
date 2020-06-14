using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace CardDealer.Model
{
    public class CardDeck : INotifyPropertyChanged
    {
        #region Local Variables
        Card[] _cardArray;
        bool _useJokers;
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

            // Populate the card array (once)
            PopulateCardArray();

            // Create the deck of cards
            Shuffle();
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
        #endregion

        #region Public Methods

        /// <summary>
        /// Shuffle the cards by creating a new card array in a randomized order and adding them to the Cards collection
        /// Also clear the Drawn Deck
        /// </summary>
        public void Shuffle()
        {
            // Create a new stack of cards in a random order
            Cards.Clear();
                        
            // Create a shuffled card array from the full deck (or full deck minus jokers if UseJokers is false)
            Random rng = new Random();
            var shuffled = (UseJokers ? _cardArray : _cardArray.Where(x => x.Suit != Card.CardSuit.Joker)).OrderBy(x => rng.Next()).ToArray();

            // Push the cards onto the card stack
            for (int i = 0; i < shuffled.Length; i++)
                Cards.Add(shuffled[i]);

            // Clear the drawn cards also
            DrawnCards.Clear();
        }

        /// <summary>
        /// Draw a card from the Cards collection and place it in the DrawnCards
        /// </summary>
        public void DrawCard()
        {
            if (Cards.Count > 0)
            {
                var card = Cards[0];
                DrawnCards.Add(card);
                Cards.RemoveAt(0);
            }
        }
        #endregion
    }
}
