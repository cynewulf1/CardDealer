using CardDealer.Model;
using System;
using System.Windows;
using System.Windows.Input;

namespace CardDealer.ViewModel
{
    public class CardDeckViewModel
    {
        // Simple ViewModel arrangement - Instantiates the Card Deck object and relays commands from the view to the model.
        // Error handling also done from here for this simple example.

        #region Private Variables        
        private ICommand _shuffleCommand, _drawCommand;
        #endregion

        #region Public Properties

        public CardDeck CardDeck { get; set; }

        public ICommand ShuffleCommand
        {
            get
            {
                return _shuffleCommand ?? (_shuffleCommand = new RelayCommand(x => {
                    try
                    {
                        CardDeck.Shuffle();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}\r\n{ex.StackTrace}", "Card Dealer");
                    }
                } ));
            }
        }
        public ICommand DrawCommand
        {
            get
            {
                return _drawCommand ?? (_drawCommand = new RelayCommand(x => {
                    try
                    {
                        CardDeck.DrawCard();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}\r\n{ex.StackTrace}", "Card Dealer");
                    }
                }));
            }
        }
        #endregion

        #region Constructor
        public CardDeckViewModel()
        {
            try
            {
                CardDeck = new CardDeck();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\r\n{ex.StackTrace}", "Card Dealer");
            }
        }
        #endregion
    }
}
