namespace CardDealer.Model
{
    public class Card
    {
        #region Enums
        public enum CardSuit { Hearts, Spades, Diamonds, Clubs, Joker }
        #endregion

        #region Properties
        // Valid range = 2-14 [2-10 numbers plus J (11), Q (12), K (13), A (14)]
        // Value for the Joker is irrelevant
        public int Value { get; set; }

        // Card suit (Hearts, Spades, Diamonds or Clubs)
        public CardSuit Suit { get; set; }
        #endregion

        #region Overrides
        // String override returns full card value and suit
        public override string ToString()
        {
            try
            {
                // Special case for Joker
                if (Suit == CardSuit.Joker)
                    return "Joker";

                string cardValue;

                if (Value >= 2 && Value <= 10)
                    cardValue = Value.ToString();
                else
                {
                    switch (Value)
                    {
                        case 11: { cardValue = "Jack"; break; }
                        case 12: { cardValue = "Queen"; break; }
                        case 13: { cardValue = "King"; break; }
                        case 14: { cardValue = "Ace"; break; }
                        default: { cardValue = ""; break; }
                    }
                }

                return $"{cardValue} of {Suit}";
            }
            catch
            {
                return base.ToString();
            }
        }
        #endregion
    }
}
